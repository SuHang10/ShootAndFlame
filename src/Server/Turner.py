# _*_ coding:utf-8 _*_


class Turner:

    def __init__(self):
        pass

    def toDict(self, msg):
        d = {}
        l = msg.split("&")
        for i in range(len(l)):
            if l[i] == '':
                continue
            s = l[i].split(",")
            d[s[0]] = s[1]
        return d

    def toString(self, msg):
        s = ""
        for k in msg:
            s = s + k + "," + str(msg[k]) + "&"
        return s[:-1]
