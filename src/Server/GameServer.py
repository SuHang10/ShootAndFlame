# _*_ coding:utf-8 _*_

import socket
import Queue
import time
import sys
from threading import Thread
from Turner import Turner
from GameThread import GameThread
from Logic.LogIn import LogIn
from Logic.Logic import Logic
from Logic.EnemyLogic import EnemyLogic

triggerManager = ["login"]


class GameServer:
    def __init__(self):
        self.close = False

        self.server = socket.socket(socket.AF_INET,socket.SOCK_DGRAM)
        self.server.bind(('127.0.0.1', 8000))
        self.triggerManager = {}
        self.logInServer = LogIn()
        self.logicServer = Logic(user_info=self.logInServer.inGameUser)
        self.enemyServer = EnemyLogic(user_info=self.logInServer.inGameUser)

        self.recvMsgThread = Thread(target=self.ServerReceiveUDP)
        self.logInThread = Thread(target=self.logInServer.loginLogic)
        self.logicThread = Thread(target=self.logicServer.mainLogic)
        self.enemyThread = Thread(target=self.enemyServer.mainLogic)
        self.recvMsgThread.start()
        self.logInThread.start()
        self.logicThread.start()
        self.enemyThread.start()

        print("UDP Server SetUp")
        """msg = {'username': 'netease1', 'password': '123',
               'address': ('127.0.0.1', 8888), 'calling': 'login'}
        self.logInServer.addTask(msg)"""

    def start(self):
        count = 0
        while not self.close:
            login_task = self.logInServer.getTask()
            enemy_task = self.enemyServer.getTask()
            try:
                while login_task.qsize() > 0:
                    task = login_task.get(block=True, timeout=0.1)#接收消息
                    if task['msg'] == "user_dead":
                        self.ServerSendUDP(task, send_all=True)
                    else:
                        self.ServerSendUDP(task)
                    login_task.task_done()

                while enemy_task.qsize() > 0:
                    task = enemy_task.get(block=True, timeout=0.1)#接收消息
                    self.ServerSendUDP(task, send_all=True)
                    enemy_task.task_done()

                count += 1
                if count/10000 == 1:
                    count = 0
                    self.logInServer.saveUserMsg()
            except Exception as e:
                pass

    def ServerReceiveUDP(self):
        while True:
            try:
                data, addr = self.server.recvfrom(2048)
                if not data:
                    print "client has exist"
                    break
                msg = turner.toDict(data)
                msg['address'] = addr
                self.RemoteCall(msg)
            except Exception as e:
                pass

    def ServerSendUDP(self, task, send_all=False):
        try:
            if send_all:
                for user in self.logInServer.userAddr:
                    addr = self.logInServer.userAddr[user]
                    msg = turner.toString(task)
                    self.server.sendto(msg, addr)
            else:
                addr = task.pop("address")
                msg = turner.toString(task)
                self.server.sendto(msg, addr)
                if task["calling"] == "login_ack" and task["msg"] == "success_login":
                    self.enemyServer.send_enemy_info()
        except Exception as e:
            print(e)

    def RemoteCall(self, msg):
        if msg['calling'] == 'login' or msg['calling'] == 'logout' or msg['calling'] == 'register'\
                or msg['calling'] == 'user_dead' or msg['calling'] == 'login_ack':
            self.logInServer.addTask(msg)
        elif msg['calling'] == 'onclip' or msg['calling'] == 'onhealth'or msg['calling'] == 'user_update':
            self.logicServer.addTask(msg)
        elif msg['calling'] == 'enemy_update' or msg['calling'] == 'enemy_dead':
            self.enemyServer.addTask(msg)
        else:
            pass


turner = Turner()
server = GameServer()
server.start()

"""t1 = Thread(target=server.ServerReceiveUDP)
t2 = Thread(target=server.ServerSendUDP)
t1.start()
t2.start()"""
