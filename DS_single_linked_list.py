'''
demo_project_Python/201115 Design linked list
Author: Elie-Yen
Python version: 3.6
'''
class Node:
    def __init__(self, val):
        self.val = val
        self.pre = None
        self.next = None

class MyLinkedList:

    def __init__(self):
        self.head = None
        self.ind = 0
        

    def get(self, index: int) -> int:
        if index == self.ind:
            return -1
        elif index < self.ind:
            t = self.head
            for _ in range(index): 
                t = t.next
            return t.val
        return -1

    def addAtHead(self, val: int) -> None:
        if not self.head: # empty list
            self.head = Node(val)
        else: # set new's next = head, head's pre = new, head = new
            new = Node(val)
            new.next, self.head.pre, self.head = self.head, new, new
        self.ind += 1
        

    def addAtTail(self, val: int) -> None:
        if not self.head: # empty list
            return self.addAtHead(val)
        else:
            new = Node(val)
            t = self.head
            while t and t.next:
                t = t.next
            new.pre, t.next = t, new
        self.ind += 1

    def addAtIndex(self, index: int, val: int) -> None:
        if index == 0:
            return self.addAtHead(val)
        elif index == self.ind:
            return self.addAtTail(val)
        elif index < self.ind:
            new = Node(val)
            t = self.head
            for _ in range(index - 1): # find the previous node of insertion
                t = t.next
            new.pre, new.next, t.next, t.next.pre = t, t.next, new, new
            self.ind += 1
            

    def deleteAtIndex(self, index: int) -> None:
        if self.head:
            if index == 0: # delete first node
                if not self.head.next: # only one node
                    self.head = None
                else:
                    self.head.next.pre, self.head,  = None, self.head.next
                self.ind -= 1
            elif index <= self.ind:
                t = self.head
                for _ in range(index - 1): # find the previous node of deletion
                    t = t.next
                # remove t.next
                if t.next:
                    t.next.pre, t.next  = t, t.next.next
                    self.ind -= 1
