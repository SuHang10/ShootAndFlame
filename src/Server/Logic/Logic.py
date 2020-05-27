# _*_ coding:utf-8 _*_

import Queue
import csv
from threading import Thread


class Logic:
    def __init__(self, user_info={}):
        self.userInfo = user_info
        self.enemyInfo = {}
        self.ack_que = Queue.Queue(32)
        self.req_que = Queue.Queue(32)

    def addTask(self, task):
        self.req_que.put(task, block=True, timeout=None)

    def getTask(self):
        return self.ack_que

    def taskDone(self):
        self.ack_que.task_done()

    def mainLogic(self):
        while True:
            try:
                task = self.req_que.get(block=True, timeout=0.1)#接收消息
                if task['calling'] == 'onhealth':
                    self.onHealth(task['chapter'], task['hp'])
                elif task['calling'] == 'onclip':
                    self.onClip(task['chapter'], task['clip'])
                elif task['calling'] == 'user_update':
                    self.user_update(task)
                self.req_que.task_done()#完成一个任务
            except Queue.Empty:
                pass

    def user_update(self, task):
        try:
            chapter = task['name']
            self.userInfo[chapter]['hp'] = task['hp']
            self.userInfo[chapter]['clip'] = task['clip']
            self.userInfo[chapter]['level'] = task['level']
            self.userInfo[chapter]['exp'] = task['exp']
            self.userInfo[chapter]['pos_x'] = float(task['pos_x'])
            self.userInfo[chapter]['pos_z'] = float(task['pos_z'])
        except Exception as e:
            print(e)

    def onHealth(self, chapter, hp):
        print("hp change = ", hp)
        self.userInfo[chapter]['hp'] = hp

    def onClip(self, chapter, clip):
        print("clip change = ", clip)
        self.userInfo[chapter]['clip'] = clip