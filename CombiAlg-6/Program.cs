using System.Collections.Generic;


HaffmanDemo2();

static void HaffmanDemo2()
{
    double[] chances;
    List<string> content;

    //string text = "zzzzabajjccbazz";
    string text = "кто пчёлок уважает" +
        "кто к ним не пристаёт" +
        "того они не жалят" +
        "тому приносят мёд";

    (chances, content) = CountChancesAndContent(text);

    var tree = Haffman(CreateNodeList(chances, content));

    PrintTree(tree.First());

    //var content = GetListOfStringChances(chances);

    Console.WriteLine(string.Join(" ", GetCodes(tree, content, false).ToList()));

    var dic = GetDictionaryCodes(tree, content);

    var codedText = CodeText(text, dic);

    Console.WriteLine(codedText);

    var coef = CoefZip(dic, chances, content);

    Console.WriteLine(coef);
    Console.WriteLine("обычное аски " + 8 * text.Length);
    Console.WriteLine("с хафменом " + (coef * text.Length));
    Console.WriteLine("с элементарными кодами " + ((coef * text.Length) + CountBit(dic)));
    //File.AppendAllText("without.txt", text);
    //File.AppendAllText("with.txt", WriteT);

}

static int CountBit(Dictionary<string, string> dic)
{
    int sum = 0;

    foreach (var key in dic.Keys)
    {
        sum += key.Length + dic[key].Length + 1 + 1; 
    }
    return sum;
}

static double CoefZip(Dictionary<string, string> dic, double[] chances, List<string> content)
{
    double sum = 0;

    for (int i = 0; i < content.Count; i++)
    {
        sum += chances[i] * (dic[content[i]].Length);
    }

    return sum;
}

static void HaffmanDemo1()
{
    double[] chances = [0.5, 0.25, 0.1, 0.1, 0.05];
    //List<string> content = ["a", "b", "c", "d", "e"];

    var tree = Haffman(CreateNodeList(chances));

    var content = GetListOfStringChances(chances);

    Console.WriteLine(string.Join(" ", GetCodes(tree, content).ToList()));

    
}

static string CodeText(string text, Dictionary<string,string> dic)
{
    string code = "";

    for (int i = 0; i < text.Length; i++)
    {
        code += dic[text[i].ToString()] + " ";
    }

    return code;
}

static (double[], List<string>) CountChancesAndContent(string text)
{
    Dictionary<char, int> map = new();

    for (int i = 0; i < text.Length; i++)
    {
        if (map.ContainsKey(text[i]))
        {
            map[text[i]]++;
        }
        else
        {
            map.Add(text[i], 1);
        }
    }

    int n = text.Length;

    var content = map.ToList().Select(el => (el.Key, el.Value)).ToList();

    content.Sort((el1,el2) => - el1.Value.CompareTo(el2.Value));

    double[] chances = new double[content.Count];

    for (int i = 0; i < content.Count; i++)
    {
        chances[i] = (content[i].Value * 1.0d)/ n;
    }

    return (chances, content.Select(el => el.Key.ToString()).ToList());
}


static Dictionary<string, string> GetDictionaryCodes(List<Node> tree, List<string> content)
{
    Dictionary<string, string> codes = new();

    for (int i = 0; i < content.Count; i++)
        if (!codes.ContainsKey(content[i]))
            codes.Add(content[i], FindWay(tree.First(), content[i]));
        
    return codes;
}


static List<string> GetCodes(List<Node> tree, List<string> content, bool ItsOnlyChances = true)
{
    List<string> codes = new List<string>();

    for (int i = 0; i < content.Count; i++)
        if(ItsOnlyChances)
            codes.Add($"{content[i].Substring(0, content[i].Length - 1)}=" + FindWay(tree.First(), content[i]));
        else
            codes.Add($"{content[i]}=" + FindWay(tree.First(), content[i]));

    return codes;
}

static List<string> GetListOfStringChances(double[] chances)
{
    List<string> content = new();

    for (int i = 0; i < chances.Length; i++)
    {
        content.Add(chances[i].ToString() + i);
    }

    return content;
}

static string? FindWay(Node node, string content, string way = "")
{
    if(node != null)
    {
        if(node.Content == "")
        {
            string? wayRight = FindWay(node.Right, content, way + "1");
            string? wayLeft = FindWay(node.Left, content, way + "0");
            if(wayRight != null)
                return wayRight;
            else
                return wayLeft;
        }
        else if(node.Content == content)
        {
            return way;
        }
    }
    return null;
}

///<summary>
///
static List<Node> CreateNodeList(double[] chances, List<string>? content = null)
{
    List<Node> nodes = new List <Node> ();

    for (int i = 0; i < chances.Length; i++)
    {
        nodes.Add(new()
        {
            Content = (content == null) ? chances[i].ToString() + i : content[i],
            Chance = chances[i]
        });
    }

    return nodes;
}

static List<Node> Haffman(List<Node> nodes)
{
    nodes.Sort((e1,e2) => - e1.Chance.CompareTo(e2.Chance));

    if(nodes.Count == 1)
    {
        return nodes;
    }

    var node1 = nodes.Last();
    nodes.RemoveAt(nodes.Count - 1);
    var node2 = nodes.Last();
    nodes.RemoveAt(nodes.Count - 1);

    nodes.Add(new()
    {
        Right = node1,
        Left = node2,
        Chance = node1.Chance + node2.Chance
    });

    return Haffman(nodes);
}

static void PrintTree(Node node, int space = 1)
{
    if (node != null)
    {
        PrintTree(node.Left, space+2);
        Console.WriteLine(new string(' ', space) + $"{string.Format("{0:f2}",node.Chance)} {node.Content}");
        PrintTree(node.Right, space + 2);

    }
}

public class Node
{
    public string Content { get; set; } = "";
    public double Chance { get; set; }
    public Node Left { get; set; } = null;
    public Node Right { get; set; } = null;
}