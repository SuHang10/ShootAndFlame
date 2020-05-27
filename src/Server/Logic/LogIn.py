# _*_ coding:utf-8 _*_

import Queue
import csv
from threading import Thread


class LogIn:
    def __init__(self):
        self.userInfo = dict()
        self.inGameUser = dict()
        with open('user.csv') as f:
            reader = csv.reader(f)
            header = next(reader)
            account_index = 0
            for i in range(len(header)):
                if header[i] == 'account':
                    account_index = i
            l = list(reader)
            f.close()
        for row in l:
            self.userInfo[row[account_index]] = {}
            for i in range(len(header)):
                self.userInfo[row[account_index]][header[i]] = row[i]

        self.userAddr = {}
        self.ack_que = Queue.Queue(32)
        self.req_que = Queue.Queue(32)

    def saveLocalMsg(self):
        try:
            out = open('user.csv', 'wb')
            csv_write = csv.writer(out)
            l = []
            key = []
            for v in self.userInfo.values():
                _list = []
                for k in v.keys():
                    if k == "pos_x" or k == "pos_y" or k == "pos_z":
                        #print(k, int(v[k]))
                        _list.append(int(v[k]))
                    else:
                        #print(k, v[k])
                        _list.append(v[k])
                l.append(_list)
                #l.append([i for i in v.values()])
                if len(key) == 0:
                    key = v.keys()
            csv_write.writerow(key)
            for row in l:
                csv_write.writerow([i for i in row])
            out.close()
        except Exception as e:
            print(e)

    def saveUserMsg(self):
        for user in self.inGameUser:
            self.userInfo[user] = self.inGameUser[user].copy()
        self.saveLocalMsg()

    def addTask(self, task):
        self.req_que.put(task, block=True, timeout=None)

    def getTask(self):
        return self.ack_que

    def taskDone(self):
        self.ack_que.task_done()

    def loginLogic(self):
        while True:
            try:
                task = self.req_que.get(block=True, timeout=0.1)#接收消息
                if task['calling'] == 'login':
                    print("log in")
                    self.login(task['username'], task['password'], task['address'])
                elif task['calling'] == 'login_ack':
                    self.login_ack(task['username'])
                elif task['calling'] == 'logout':
                    self.logout(task['username'])
                elif task['calling'] == 'register':
                    self.register(task['username'], task['password'], task['address'])
                elif task['calling'] == 'user_dead':
                    self.user_dead(task)
                self.req_que.task_done()#完成一个任务
            except Queue.Empty:
                pass

    def login(self, username, password, addr):
        print("login service")
        login_ack = {}
        login_ack["calling"] = "login_ack"
        login_ack["address"] = addr
        if username in self.userAddr.keys():
            print(username, "User Has Log In")
            login_ack["msg"] = "has_login"
            login_ack["state"] = "0"
        else:
            if username in self.userInfo.keys() and self.userInfo[username]['password'] == password:
                print(username, "Log In Success")
                self.userAddr[username] = addr
                self.inGameUser[username] = self.userInfo[username].copy()
                login_ack["msg"] = "success_login"
                login_ack["state"] = "1"
                for k in self.userInfo[username]:
                    login_ack[k] = self.userInfo[username][k]
                level = int(login_ack["level"])
                login_ack["damage"] = str(20 + (level-1) * 5)
            elif username in self.userInfo.keys():
                print(username, "Wrong Password")
                login_ack["msg"] = "wrong_password"
                login_ack["state"] = "0"
            else:
                print(username, "Wrong Username")
                login_ack["msg"] = "wrong_username"
                login_ack["state"] = "0"
        self.ack_que.put(login_ack, block=True, timeout=None)

    def login_ack(self, username):
        self.inGameUser[username] = self.userInfo[username].copy()

    def logout(self, username):
        try:
            self.inGameUser.pop(username)
            self.userAddr.pop(username)
            print(username, "Log Out Success")
        except:
            pass

    def register(self, username, password, addr):
        register_ack = {}
        register_ack["calling"] = "register_ack"
        register_ack["address"] = addr
        if username in self.userInfo.keys():
            register_ack["msg"] = "has_register"
            register_ack["state"] = "0"
            print(username, "User Has Register")
        else:
            register_ack["msg"] = "success_register"
            register_ack["state"] = "1"
            self.userInfo[username] = {}
            self.userInfo[username]['username'] = username
            self.userInfo[username]['password'] = password
            self.userInfo[username]['hp'] = 2000
            self.userInfo[username]['clip'] = 200
            self.userInfo[username]['level'] = 1
            self.userInfo[username]['exp'] = 0
            self.userInfo[username]['pos_x'] = 0
            self.userInfo[username]['pos_z'] = 0
            self.saveUserMsg()
            print(username, "Register Success")
        self.ack_que.put(register_ack, block=True, timeout=None)

    def user_dead(self, task):
        try:
            username = task['name']
            self.userInfo[username]['hp'] = 2000 + 200 * (int(task['level'])-1)
            self.userInfo[username]['clip'] = 200
            self.userInfo[username]['level'] = task['level']
            self.userInfo[username]['exp'] = task['exp']
            self.userInfo[username]['pos_x'] = 0
            self.userInfo[username]['pos_z'] = 0
            self.inGameUser.pop(username)
            self.userAddr.pop(username)
            #self.saveLocalMsg()
            print(username, "Dead")
            print(self.userInfo[username])
            dead_ack = dict()
            dead_ack["msg"] = "user_dead"
            dead_ack["name"] = task['name']
            self.ack_que.put(dead_ack, block=True, timeout=None)
        except Exception as e:
            print(e)
