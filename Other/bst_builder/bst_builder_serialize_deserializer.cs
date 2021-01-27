/*------
C#
Elie yen
bst builder

@value of node: int (range between (+/-)2^31 - 1), duplicate is not allowed
@the format of string input/ output is a level-order representation eg:
[5 | 3, 8 | 2, 4, 7, 9 | 1, null, null, null, 6, null, null, null]
which '|' is a separator of different level and show null
------*/

// constructor of node
public class TNode {
    public int val;
    public int height = 1;
    public TNode left;
    public TNode right;
    public TNode(int x) { val = x; }
}

public class BSTbuilder {
    private TNode _root;
    private int _count = 0; // number of non-null elements

    // caculation for creating BST and maintaining AVL tree
    private int GetHeight(TNode node) { return (node == null)? 0: node.height;  }   
    private int UpdateHeightbychild(TNode node){
        // use height of node's children to calculate height
        return Math.Max(GetHeight(node.left), GetHeight(node.right)) + 1;
    }
    private TNode RightRotate(TNode node){
        // rotate right by use node.left as new _root
        TNode L = node.left;
        TNode LR = L.right;
        L.right = node;
        node.left = LR;
        // update height
        node.height = UpdateHeightbychild(node);
        L.height = UpdateHeightbychild(L); 
        return L;
    }
    private TNode LeftRotate(TNode node){
        // same as RightRotate
        TNode R = node.right;
        TNode RL = R.left;
        R.left = node;
        node.right = RL;
        node.height = UpdateHeightbychild(node);
        R.height = UpdateHeightbychild(R); 
        return R;
    }
    private int GetDifference(TNode node){
        return (node == null)? 0:
            GetHeight(node.left) - GetHeight(node.right);
    }
    private bool ValidBST(){
        // set boundary by int64 to avoid false judgement
        // made of comparing a node's val to int.MaxValue
        bool helper(TNode node, long max, long min){
            if (node == null){
                return true;
            }
            if (node.val <= min || node.val >= max){
                return false;
            }
            bool L = helper(node.left, node.val, min);
            bool R = helper(node.right, max, node.val);
            if (L && R){
                return true;
            }
            return false;
        }
        return helper(_root, long.MaxValue, long.MinValue);
    }
    private TNode RebuildBST(int startidx, int endidx, List<int> sortedvalue){
        // this is for constructors failed to build a valid BST
        // as a result of duplicates or violation BST rules
        // array must be preprocessed to become sorted and has only unique elements.
        // build balanced BST recursively
        // bottom-up, use its children to calculate height
        if (endidx == startidx){
            return null;
        }
        if (endidx - startidx == 1){
            TNode res1 = new TNode(sortedvalue[startidx]);
            return res1;
        }
        int m = (int)(startidx + (endidx - startidx) * 0.5);
        TNode res = new TNode(sortedvalue[m]);
        res.left = RebuildBST(startidx, m, sortedvalue);
        res.right = RebuildBST(m + 1, endidx, sortedvalue);
        res.height = UpdateHeightbychild(res);
        return res;
    }
    private List<int> ToList(SortedSet<int> vset){
        // turn into List which is indexed for rebuild
        List<int> res = new List<int>();
        foreach (int v in vset){
            res.Add(v);
        }
        return res;
    }

    // properties
    public TNode root { get => _root; }
    public int count { get => _count; }
    public int height { get{ return GetHeight(_root); } }
    public override string ToString(){
        // represented by level order, shows null
        string res = "[";
        if (_root == null){
            res += "null]";
        }
        else {
            Queue<TNode> que = new Queue<TNode>();
            que.Enqueue(_root);
            bool notonlynull = true; // trim the level that only has null
            while (notonlynull){
                notonlynull = false;
                int m = que.Count;
                string thislevel = "";
                for (int i = 0; i < m; i++){
                    TNode node = que.Dequeue();
                    if (node != null){
                        notonlynull = true;
                        thislevel += $"{node.val}, ";
                        que.Enqueue(node.left);
                        que.Enqueue(node.right);
                    }
                    else{
                        thislevel += "null, ";
                    }
                }
                res += (notonlynull)? thislevel.TrimEnd(',', ' ') + " | " : "";
            }
            res = res.TrimEnd(',', ' ', '|') + "]";
        }
        return res;
    }

    // other operations
    public bool Insert(int _value){
        // return true if no duplicate in _root
        bool helper(int v, ref TNode node){
            if (node == null){
                node = new TNode(v);
                _count ++;
                return true;
            }
            if (v == node.val){
                return false;
            }
            // get the boolean and update children first
            bool res = (v > node.val)? helper(v, ref node.right): helper(v, ref node.left);
            
            // update height
            node.height = UpdateHeightbychild(node);
            int dif = GetDifference(node);

            // rightrotate when left is higher (has less depth)
            if (dif > 1){
                node.left = (node.left.val < v)? LeftRotate(node.left): node.left;
                node = RightRotate(node);
            }
            else if (dif < -1){
                node.right = (node.right.val > v)? RightRotate(node.right): node.right;
                node = LeftRotate(node);
            }
            return res;
        }
        return helper(_value, ref _root);
    }
    public void Delete(int _value){
        // it will be ok if the deletion target isn't in root.
        TNode helper(int v, TNode node){
            if (node == null){
                return node;
            }
            // reach the target
            if (node.val == v){
                // when has 0 or 1 child
                if (node.left == null && node.right == null){
                    _count --;
                    node = null;
                    return node;
                }
                else if (node.left == null){
                    _count --;
                    node = node.right;
                }
                else if (node.right == null){
                    _count --;
                    node = node.left;
                }
                // has 2 children
                else {
                    // replace it with its inorder successor
                    TNode successor = node.right;
                    while (successor.left != null){
                        successor = successor.left;
                    }
                    // replace
                    node.val = successor.val;
                    // delete successor
                    node.right = helper(node.val, node.right);
                }
            }
            // continue to find target
            else if (node.val > v){
                node.left = helper(v, node.left);
            }
            else {
                node.right = helper(v, node.right);
            }

            // update height & rotate, resemble to insertion
            node.height = UpdateHeightbychild(node);
            int diff = GetDifference(node);
            if (diff > 1){
                node.left = (GetDifference(node.left) < 0)?
                            LeftRotate(node.left): node.left;
                return RightRotate(node);
            }
            if (diff < -1){
                node.right = (GetDifference(node.right) > 0)?
                            RightRotate(node.right): node.right;
                return LeftRotate(node);
            }
            return node;
        }

        _root = helper(_value, _root);
    }
    public bool Contains(int _value){
        // like insert, but looking for value only
        bool helper(int v, TNode node){
            if (node == null){
                return false;
            }
            if (v == node.val){
                return true;
            }
            if (v > node.val){
                return helper(v, node.right);
            }
            return helper(v, node.left);
        }
        return helper(_value, _root);
    }
    
    // serialize, by different traversal way (ignore null)
    public int[] GetInOrder(){
        // get recursively
        List<int> inorder_traversal(TNode node){
            List<int> res = new List<int>();
            if (node == null){
                return res;
            } 
            res.AddRange(inorder_traversal(node.left));
            res.Add(node.val);
            res.AddRange(inorder_traversal(node.right));
            return res;
        }
        return inorder_traversal(_root).ToArray();
    }
    public int[] GetPreOrder(){
        List<int> preorder_traversal(TNode node){
            List<int> res = new List<int>();
            if (node == null){
                return res;
            } 
            res.Add(node.val);
            res.AddRange(preorder_traversal(node.left));
            res.AddRange(preorder_traversal(node.right));
            return res;
        }
        return preorder_traversal(_root).ToArray();
    }
    public int[] GetPostOrder(){
        List<int> postorder_traversal(TNode node){
            List<int> res = new List<int>();
            if (node == null){
                return res;
            } 
            res.AddRange(postorder_traversal(node.left));
            res.AddRange(postorder_traversal(node.right));
            res.Add(node.val);
            return res;
        }
        return postorder_traversal(_root).ToArray();
    }
    public int[][] GetLevelOrder(){
        List<int[]> res = new List<int[]>();
        if (_root == null){
            return res.ToArray();
        }
        Queue<TNode> que = new Queue<TNode>();
        que.Enqueue(_root);
        while (que.Count > 0){
            int m = que.Count;
            List<int> thislevel = new List<int>();
            for (int i = 0; i < m; i++){
                TNode node = que.Dequeue();
                thislevel.Add(node.val);
                if (node.left != null){          
                    que.Enqueue(node.left);
                }
                if (node.right != null){          
                    que.Enqueue(node.right);
                }
            }
            res.Add(thislevel.ToArray());
        }
        return res.ToArray();
    }
    public string OrderVisualize(string order){
        // this is for all traversal way, but won't show null
        int[] values;
        string res = "";
        switch (order.ToLower()){
            case "inorder": values = this.GetInOrder(); break;
            case "preorder": values = this.GetPreOrder(); break;
            case "postorder": values = this.GetPostOrder(); break;
            case "levelorder":
                res = $"{order} : {this.ToString()} , height: {this.height}, non-null elements: {this.count}";
                return ;
            default: Console.WriteLine("Error, invalid input."); return res;
        }
        foreach (int v in values){
            res += $"{v}, ";
        }
        res = $"{order} : [ {res.TrimEnd(',', ' ')} ] , height: {this.height}, non-null elements: {this.count}";
        return res;
    }

    // constructors
    public BSTbuilder(int[] values){
        // remove duplicates and sort
        SortedSet<int> vset = new SortedSet<int>(values);
        List<int> sortedvalue = ToList(vset);
        _count = sortedvalue.Count;
        _root = RebuildBST(0, _count, sortedvalue);
    }
    public BSTbuilder(TNode rt){
        // this rebuilds a BST if rt is not a valid BST
        _root = rt;
        if (! ValidBST()){
            Console.WriteLine("this is not a BST, but we will fix it");
            SortedSet<int> vset = new SortedSet<int>(this.GetInOrder());
            List<int> sortedvalue = ToList(vset);
            _count = sortedvalue.Count;
            _root = RebuildBST(0, _count, sortedvalue);
        }
    } 
    public BSTbuilder(string s){
        // can also be seem as deserializer
        // use the same format as ToString()
        s = s.Replace(" ", "").Trim('[', ']');
        if (s.Length == 0 || s == "null"){
            _root = null;
            return ;
        }
        string[] subs = s.Split('|');
        int n = subs.Length - 1;
        Queue<TNode> bottomlevel = new Queue<TNode>();
        SortedSet<int> vset = new SortedSet<int>(); // for rebuilt if it's not a BST

        // build by bottom-up to calculate height
        for (int i = n; i > -1; i--){
            Queue<TNode> upperlevel = new Queue<TNode>();
            foreach (string v in subs[i].Split(',')){
                try {
                    TNode tmp = (v == "null")? null: new TNode(int.Parse(v));
                    if (tmp != null){
                        if (i < n){
                            tmp.left = bottomlevel.Dequeue();
                            tmp.right = bottomlevel.Dequeue();
                            tmp.height = UpdateHeightbychild(tmp);
                        }
                        vset.Add(tmp.val);
                        _count ++;
                    }
                    upperlevel.Enqueue(tmp);
                }
                catch (System.FormatException){
                    Console.WriteLine("Error, value must be an integer or null");
                }
                catch (InvalidOperationException){
                    Console.WriteLine("Error, do you miss [ null ] ?" +
                    " Ensure it's an level order and every parent node should have 2 child node");
                }
            }
            bottomlevel = upperlevel;
        }
        if (bottomlevel.Count == 1){
            _root = bottomlevel.Dequeue();
            if (! ValidBST()){
                Console.WriteLine("this is not a BST, but we will fix it");
                List<int> sortedvalue = ToList(vset);
                _count = sortedvalue.Count;
                _root = RebuildBST(0, _count, sortedvalue);
            }
        }
        else{
            throw  new System.FormatException("Error, the first level must contain only one value" +
                    " which must be an integer, not null");
        }
    }

}

class Test {
    static void Main() {
        int[] values = new int[]{1,2,3,4,5,6,7,8,9};
        var ex = new BSTbuilder(values);
        Console.WriteLine(ex); 
        // [5 / 3, 8 / 2, 4, 7, 9 / 1, null, null, null, 6, null, null, null]
        var ex2 = new BSTbuilder("[5|5,99|42,4,78,9|1,null,null,null,6,null,null,null]"); // not valid, rebuild
        Console.WriteLine(ex2); 
        // [9 | 5, 78 | 4, 6, 42, 99 | 1, null, null, null, null, null, null, null]
        
        ex2.Delete(11); // this would do nothing since 11 is not in root
        ex2.Delete(4);
        Console.WriteLine(ex2.Insert(5)); // false, since 5 already in root
        ex2.Insert(55);
        Console.WriteLine(ex2);
         // [9 | 5, 78 | 1, 6, 42, 99 | null, null, null, null, null, 55, null, null]
        Console.WriteLine(ex2.OrderVisualize("preorder"));
        // preorder : [ 9, 5, 1, 6, 78, 42, 55, 99 ] , height: 4, non-null elements: 8
    }
}
