# _*_ coding:utf-8 _*_
import Queue
import time
from threading import Thread


class GameThread(Thread):
    def __init__(self, _queue=None, target=None, args=None, kwargs=None):
        self.queue = _queue
        self.__target = target
        self.__args = args
        self.__kwargs = kwargs
        super(GameThread, self).__init__(target=target)

    def run(self):
        if self.__target:
            self.__target()

        """while not self.thread_stop:
            try:
                task = self.queue.get(block=True, timeout=20)#接收消息
            except Queue.Empty:
                print("Nothing to do!i will go home!")
                self.thread_stop=True
                break
            print("task recv:%s ,task No:%d" % (task[0],task[1]))
            print("i am working")
            time.sleep(3)
            print("work finished!")
            self.queue.task_done()#完成一个任务
            res = self.queue.qsize()#判断消息队列大小
            if res > 0:
                print("fuck!There are still %d tasks to do" % (res))"""


if __name__ == "__main__":
    q = Queue.Queue(10)
    worker=GameThread(q)
    worker.start()
    q.put(["produce one cup!",1], block=True, timeout=None)#产生任务消息
    q.put(["produce one desk!",2], block=True, timeout=None)
    q.put(["produce one apple!",3], block=True, timeout=None)
    q.put(["produce one banana!",4], block=True, timeout=None)
    q.put(["produce one bag!",5], block=True, timeout=None)
    q.put(["produce one bag!",6], block=True, timeout=None)
    q.put(["produce one bag!",7], block=True, timeout=None)
    q.put(["produce one bag!",8], block=True, timeout=None)
    q.put(["produce one bag!",9], block=True, timeout=None)
    q.put(["produce one bag!",10], block=True, timeout=None)
    q.put(["produce one bag!",11], block=True, timeout=None)
    q.put(["produce one bag!",12], block=True, timeout=None)
    q.put(["produce one bag!",13], block=True, timeout=None)
    #print("***************leader:wait for finish!")
    q.join()#等待所有任务完成
    #print("***************leader:all task finished!")