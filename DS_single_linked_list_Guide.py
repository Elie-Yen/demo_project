'''
demo_project_Python/201115 Design linked list
Author: Elie-Yen
Python version: 3.6
'''

# step 1: create a method for linking
'''
As title, here I named it '**Node**'.
We would need it when we want a **value(val)** to link to **next** - 
here it's something could be None, another value, or another Node object, depending on its position. 
If we have not decide what this value to link (when we start build it or the last one), 
we just simply assign its *next* is None.
Now it has both *val* and *next*, it's called a *Node object*.
'''

	class Node(object):
		def __init__(self, val):
			self.val = val
			self.next = None

# step 2: initializing
'''
We need a start which says : 'Here is the head of MyLinkedList !'.
Here I name it **head**, and assign it to None.
Besides, we also need something to help indexing this list. 
Therefore, I create **id** to indicate the length of it. 
**(attention:** it's better named it ind to distinguish it from built-in function id())
I also creat a list for debugging.
'''

	class MyLinkedList: # construct the data structure
		def __init__(self): 
			self.head = None
			self.id = 0 # index for head, also the length of mylist
			#self.list =[] # debug mode, remove # at each method to enable
    
# step 3: add items
'''
(In my personal opinion, I change the order of default codes to let it more easy.)
First, create a Node object **node (nodeobject)**, whose *val* = val, in order to link to next element.
'''
        # Add at head
		def addAtHead(self, val: int) -> None:
			node = Node(val)
			if not self.head: # add to empty list
				self.head = node
				#self.list.append(val)
			else: # add to exist list
				self.head, node.next = node, self.head
				#self.list = [val] + self.list
			self.id += 1

			#self.debug()
    '''
	When it comes to add at head, it's a little tricky:
	1.  add to empty list:  
		just announce the head is the *nodeobject* now.
	2.  add to non-empty list: 
		like 1, the head is the *node* now, but how about the **original head**? 
		Since the nodeobject is the first element now, **the original head now becomes nodeobject's *next*** .

	Because an item is added, we need to increase id with 1 to change the length at the same time.
    '''

        # Add at tail
		def addAtTail(self, val: int) -> None:
			if not self.head:
				self.addAtHead(val)
			else:
				pre = self.head
				for _ in range(self.id - 1): # find previous node of tail(=None)
					pre = pre.next
				pre.next = Node(val)
				self.id += 1

				#self.list.append(val)
				#self.debug()
    
    '''
	When there's no *head*,  it means *nodeobject* is the first elements, just like add at head.
	In other situation, We need to **find the previous node before the tail** 
    (since the tail (last element) is always None, None does't have *next* attribute), 
	so we loop untill we arrive at **id (length of the list) - 2** 
    (extra 1 because the result is the node's *next*). 
    After that, we assign this previous node's *next* to *nodeobject*.
	'''
        # Add at index
		def addAtIndex(self, index: int, val: int) -> None:
				if index == 0:
					self.addAtHead(val)
				elif index <= self.id:
					node = Node(val)
					pre = self.head
					for _ in range(index - 1): # find previous node (before the index)
						pre = pre.next
					#pre -> node -> original pre.next(= current node.next)
					pre.next, node.next = node, pre.next
					self.id += 1

					#self.list.insert(index, val)
					#self.debug()
    '''
	Just like AddAtTail, we need to loop first to find the previous node. 
	However, this time, we **linked previous node to *nodeobject*, *nodeobject* to original  previous node's *next*.**
    '''

# step 4: delete items
'''
Just like AddAtIndex, however :
1. no need to create a new *nodeobject*, we just recombine it
2. **the index cannot equal to id**, since it means to delete the last element - None, and it would raise an error

If we want to delete the first element, we need to assign the head to itself's *next*.
In other situaitons, we loop to find the previous node of where we'd like to delete, 
and **link previous node to *next* of itself's *next*** (element after the delete element)
Because an item is deleted, we need to decrease id with 1 to change the length at the same time.
'''
    def deleteAtIndex(self, index: int) -> None:
        if index < self.id:
            pre = self.head
            for _ in range(index - 1): # find previous node of where to delete
                pre = pre.next
            # pre -> original pre.next.next(= current pre.next)
            if index == 0 and pre:
                self.head = pre.next
            elif pre.next:
                pre.next = pre.next.next
            self.id -= 1
            
            #del self.list[index]
            #self.debug()
			
# step 5: get items and debug
'''
Just like what we did to find index-th element above, but we return its *val* this time.
'''
    def get(self, index: int) -> int:
        # if index == self.id, the elements will be None which doesn't have val 
        if index < self.id: 
            x = self.head
            for _ in range(index):
                x = x.next 
            #self.debug()
            return x.val
        return -1
		
'''
Finally ! here's the code I wrote to debug, it produces 2 lists at steps you want:
1. what this list should look like
2.  what it really looks like
'''
		def debug(self) -> None:
			x = self.head
			L = []
			for _ in range(self.id):
				if x:
					L.append(x.val)
					x = x.next
				else:
					L.append(x)
			print(self.list,'*', L)
    '''			
	It will like the cell below, you'll know something wrong when seeing difference between 2 lists.

		[1] * [1]
		[1, 3] * [1, 3]
		[1, 2, 3] * [1, 3, 2]  << something wrong
		[1, 2, 3] * [1, 3, 2]
		[1, 3] * [1, 2]
		[1, 3] * [1, 2]
    '''
