// 我真诚地保证：

// 我自己独立地完成了整个程序从分析、设计到编码的所有工作。
// 如果在上述过程中，我遇到了什么困难而求教于人，那么，我将在程序实习报告中
// 详细地列举我所遇到的问题，以及别人给我的提示。

// 在此，我感谢Microsoft Corporation对我的启发和帮助。下面的报告中，我还会具体地提到
// 他们在各个方法对我的帮助。

// 我的程序里中凡是引用到其他程序或文档之处，
// 例如教材、课堂笔记、网上的源代码以及其他参考书上的代码段,
// 我都已经在程序的注释里很清楚地注明了引用的出处。

// 我从未没抄袭过别人的程序，也没有盗用别人的程序，
// 不管是修改式的抄袭还是原封不动的抄袭。

// 我编写这个程序，从来没有想过要去破坏或妨碍其他计算机系统的正常运转。

// <耿瑞>

//本程序的部分语法参考了Microsoft Corporation的MSDN文档

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class Program
    {
        static void Main(string[] args)
        {
            AdjList graph = new AdjList();  //实例化邻接表
            string Input;

            //读入边 直至读入'#'或空行
            for (; ; )
            {
                Input = Console.ReadLine();
                if (Input == "#" || Input == "")
                {
                    break;
                }
                else
                {
                    graph.Add(Input);   //添加边
                }
            }

            //输出建立的邻接表的示意图
            foreach (AdjNode n in graph.Vertices)
            {
                var temp = n;
                Console.Write(n.Content + ": ");
                while (temp.Next != null)
                {
                    Console.Write(temp.Next.Content + "/" + temp.Next.Weight + " ");
                    temp = temp.Next;
                }
                Console.WriteLine();
            }

            graph.DFS();    //深度优先搜索
            graph.BFS();    //广度优先搜索

            //读入要查找的路径
            Input = Console.ReadLine();
            string X = Input.Split(' ')[0];                 //始顶点
            string Y = Input.Split(' ')[1];                 //终顶点
            int Distence = int.Parse(Input.Split(' ')[2]);  //路径长度
            graph.FindPath(X, Y, Distence); //查找路径
            Console.ReadLine();
        }
    }

    //AdjNode类 邻接表节点
    class AdjNode
    {
        public string Content;      //顶点标识
        public int Weight;          //边权重
        public AdjNode Next = null; //下一邻接节点

        //构造方法 参数为空的重载
        public AdjNode()
        {
            Content = null;
            Weight = -1;
        }

        //构造方法 参数为标识的重载
        public AdjNode(string content)
        {
            Content = content;
            Weight = -1;
        }

        //构造方法 参数为标识与权重的重载
        public AdjNode(string content, int weight)
        {
            Content = content;
            Weight = weight;
        }
    }

    //AdjList类 邻接表
    class AdjList
    {
        public List<AdjNode> Vertices = new List<AdjNode>();    //用于储存图顶点的List

        //构造方法
        public AdjList()
        {
        }

        //Add方法 新增一条边
        public void Add(string input)
        {
            string x = input.Split(' ')[0];                 //始顶点
            string y = input.Split(' ')[1];                 //终顶点
            int weight = int.Parse(input.Split(' ')[2]);    //边权重
            if (Vertices.Find(v => v.Content == x) != default(AdjNode))  //此处使用Lambda表达式从Vertices中获得始顶点x的位置
            //若Vertices中已经存在顶点x则直接向其所指向的邻接链表的末端增加一个节点
            {
                AdjNode temp = Vertices.Find(v => v.Content == x);
                while (temp.Next != null)
                {
                    temp = temp.Next;
                }
                temp.Next = new AdjNode(y, weight);
            }
            else
            //若Vertices中不存在顶点x则新增一个顶点并在其所指向的邻接链表末端增加一个节点
            {
                AdjNode temp = new AdjNode(x);
                Vertices.Add(temp);
                temp.Next = new AdjNode(y, weight);
            }
        }

        //Dfs方法 深度优先搜索递归体
        private void Dfs(AdjNode n)
        {
            n.Weight = (int)Status.Visited; //标记顶点为访问结束 访问状态存储于Vertices中顶点元素的Weight变量
            Console.Write(n.Content + " "); //输出当前顶点标识
            AdjNode temp = n.Next;
            while (temp != null)
            {
                if (Vertices.Find(v => v.Content == temp.Content).Weight == (int)Status.NotDetected) //若顶点未被探测
                {
                    Dfs(Vertices.Find(v => v.Content == temp.Content));     //递归搜索顶点
                }
                temp = temp.Next;
            }
        }

        //DFS方法 深度优先搜索调用体
        public void DFS()
        {
            IniVisited();   //初始化访问状态
            Console.Write("DFS: ");
            Dfs(Vertices.First());  //自Vertices中的第一个顶点开始深度优先搜索
            Console.WriteLine();
        }

        //Bfs方法 广度优先搜索递归体
        private void Bfs(AdjNode n)
        {
            if (n.Weight == (int)Status.NotDetected) //若顶点未被探测
            {
                Console.Write(n.Content + " "); //输出当前顶点标识
            }
            n.Weight = (int)Status.Visited; //标记顶点为访问结束
            AdjNode temp = n.Next;
            while (temp != null)
            {
                if (Vertices.Find(v => v.Content == temp.Content).Weight != (int)Status.Visited)    //若顶点未完成访问
                {
                    if (Vertices.Find(v => v.Content == temp.Content).Weight != (int)Status.Detected)   //若顶点未被探测
                    {
                        Vertices.Find(v => v.Content == temp.Content).Weight = (int)Status.Detected;    //标记顶点为已探测
                        Console.Write(temp.Content + " ");  //输出当前顶点标识
                    }
                }
                temp = temp.Next;
            }
            temp = n.Next;
            while (temp != null)
            {
                if (Vertices.Find(v => v.Content == temp.Content).Weight == (int)Status.Detected)    //若顶点已被探测
                {
                    Bfs(Vertices.Find(v => v.Content == temp.Content)); //递归搜索顶点
                }
                temp = temp.Next;
            }
        }

        //BFS方法 广度优先搜索调用体
        public void BFS()
        {
            IniVisited();   //初始化访问状态
            Console.Write("BFS: ");
            Bfs(Vertices.First());  //自Vertices中的第一个顶点开始广度优先搜索
            Console.WriteLine();
        }

        Stack<string> path = new Stack<string>();   //用于存储以搜索路径的堆栈
        int Len;    //路径长度
        string Ops; //目标顶点标识

        //Findpath方法 查找两顶点间定常路径(递归体)
        private void Findpath(AdjNode n, int level)
        {
            if (level == Len)    //搜索层级等于路径长度
            {
                if (n.Content == Ops)    //当前顶点为目标顶点
                {
                    //将路径堆栈元素转换为一条路径输出
                    string[] output = path.ToArray();
                    for (int i = output.Length - 1; i >= 0; i--)
                    {
                        Console.Write(output[i]);
                        if (i != 0)
                        {
                            Console.Write("->");
                        }
                    }
                    Console.WriteLine();
                }
                path.Pop(); //弹出栈顶顶点
                return;
            }
            AdjNode temp = n.Next;
            while (temp != null)
            {
                if (path.Contains(temp.Content)) //若路径中已含有此顶点(成环)
                {
                    temp = temp.Next;   //跳过此顶点
                    continue;
                }
                path.Push(temp.Content);    //压入此顶点
                Findpath(Vertices.Find(v => v.Content == temp.Content), level + 1); //递归搜索此顶点的邻接顶点
                temp = temp.Next;
            }
        }

        //FindPath方法 查找两顶点间定常路径(调用体)
        public void FindPath(string x, string y, int len)
        {
            IniVisited();   //初始化访问状态
            Len = len;
            Ops = y;
            path.Clear();   //清空路径堆栈
            path.Push(x);   //压入始节点
            Findpath(Vertices.Find(v => v.Content == x), 0);    //自始顶点开始递归查找路径
        }

        //IniVisited方法 初始化所有节点的访问状态为未发现
        public void IniVisited()
        {
            foreach (AdjNode n in Vertices)
            {
                n.Weight = (int)Status.NotDetected;
            }
        }

        //Status枚举 节点的访问状态
        enum Status
        {
            Visited = -2,       //访问结束
            NotDetected = -1,   //未探测
            Detected = -3       //已探测
        }
    }
}
