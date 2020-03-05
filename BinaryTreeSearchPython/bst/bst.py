import queue

class Node:
    def __init__(self, name):
        self.name = name
        self.neighbours = []

    def __str__(self):
        return self.name

    def __repr__(self):
        return self.name

class BST:
    def __init__(self):
        self.nodes = {
            "0" : Node("0"),
            "1" : Node("1"),
            "2" : Node("2"),
            "3" : Node("3")}
        self.buildNodeTree()

   
 #                   (0*)
 #                 /    \
 #              (1)------(2*)
 #                         \
 #                          (3*)
    def buildNodeTree(self):
        self.nodes["0"].neighbours = ["1", "2"]
        self.nodes["1"].neighbours = ["0", "2"]
        self.nodes["2"].neighbours = ["1", "3"]
        self.nodes["3"].neighbours = ["2"]

    def findShortestRoute(self, start, end):
        nodeStart = self.nodes[start]
        nodeEnd = self.nodes[end]

        if end in nodeStart.neighbours:
            return [nodeStart, nodeEnd]

        lastNode = None

        nodeMap = {}
        toVisit = queue.Queue()

        nodeMap[nodeStart.name] = (None, 0)
        toVisit.put(nodeStart)
        
        while not toVisit.empty():
            e = toVisit.get()
            distance = nodeMap[e.name][1]

            for n in e.neighbours:

                node = self.nodes[n]

                if lastNode is node:
                    continue

                if node.name not in nodeMap:
                    nodeMap[node.name] = (e, distance +1)
                    toVisit.put(node)
                else:
                    t = nodeMap[node.name]

                    if distance +1 < t[1]:
                        nodeMap[e.name] = (node, distance+1)

            lastNode = e

        results = []
        current = nodeEnd
        
        while current is not None:
            if current.name not in nodeMap:
                return (nodeMap, "No route found")
            
            results.insert(0, current)
            current = nodeMap[current.name][0]

        return results