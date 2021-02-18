/*--
Elie Yen
C#
Password generator
--*/
class Password {
    private int _length;
    private bool _uppercase;
    private bool _lowercase;
    private bool _digit;
    private bool _symbol;
    private bool _space;
    private string _other;
    private int _permutation_time;
    
    public int length { get => _length; set{_length = value;} }
    public bool uppercase { get => _uppercase; set{_uppercase = value;} }
    public bool lowercase { get => _lowercase; set{_lowercase = value;} }
    public bool digit { get => _digit; set{_digit = value;} }
    public bool symbol { get => _symbol; set{_symbol = value;} }
    public bool space { get => _space; set{_space = value;} }
    public string other { get => _other; set{_other = value;} }
    public int permutation_time { get => _permutation_time; set{_permutation_time = value;} }

    public Password(
        int len = 16,  bool upper = true,  bool lower = true,  bool num = true,  bool special = false,  bool spce = false,  string oth = "" ,  int permutation = 0){
        Console.WriteLine(@"Welcome! Generate customized password by setting attributes below:
        Set a int for the length of your password : [length]. (5 < value < 100)
        Set true if used otherwise false: [uppercase], [lowercase], [digit], [symbol], [space].
        Set a string if the chars you want to used are not in the list above: [other].
        Set a int if you want to permutate the result: [permutation_time]. (0 < value < (length !) )

        Default attrubutes: length = 16, use only uppercases, lowercases and digits without permutation.
        If you set up already, use [ Generate() ] to generate.");
        
        length = len;
        uppercase = upper;
        lowercase = lower;
        digit = num;
        symbol = special;
        space = spce;
        other = oth;
        permutation_time = permutation;

        string sample = Generate();
        Console.WriteLine($"This is sample: {sample}");
    }

    public String Generate(){
        Dictionary<int, string> charset = new Dictionary<int, string>{
                [0] = "abcdefghijklmnopqrstuvwxyz",
                [1] = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", 
                [2] = "0123456789",           
                [3] = "~!@#$%^&*_-+=`|(){}[]:;\"'<>,.?/\\",
                [4] = "          ",
                [5] = other
        };    
        String res = "";
        bool[] option = new bool[]{lowercase, uppercase, digit, symbol, space, other.Length > 0};
        List<string> used = new List<string>();
        for (int i = 0; i < 6; i++){
            if (option[i]){
                used.Add(charset[i]);
            }
        }
        // valid inputs
        if (length < 6){
            Console.WriteLine("Error, Set higher length for your security.");
        }
        else if (used.Count < 2){
            Console.WriteLine(
                "Error, must choose at least 2 types of character to generate the password.");
        }
        else {
            StringBuilder password = new StringBuilder("");
            //Random rnd = new Random();

            // the position to be filled
            List<int> pos = new List<int>();
            for (int i = 0; i < length; i++){
                pos.Add(i);
                password.Append("_");
            }

            int unfilled = length;
            int avgtime = (length / used.Count > 2)? length / used.Count : 1;
            
            // ensure at least one required char is used
            foreach (string chars in used){
                for (int times = 0; times < avgtime ; times++){
                    int x = System.Security.Cryptography.RandomNumberGenerator.GetInt32(pos.Count);
                    int j = pos[x];
                    pos.Remove(j);
                    int m = System.Security.Cryptography.RandomNumberGenerator.GetInt32(chars.Length);
                    password[j] = chars[m];
                    }
                unfilled -= avgtime;
            }

            // randomly pick from used charset
            for (int i = 0; i < unfilled; i++){
                int x = System.Security.Cryptography.RandomNumberGenerator.GetInt32(pos.Count);
                int j = pos[x];
                pos.Remove(j);
                int k = System.Security.Cryptography.RandomNumberGenerator.GetInt32(used.Count);
                int m = System.Security.Cryptography.RandomNumberGenerator.GetInt32(used[k].Length);
                password[j] = used[k][m];
            }
            res = password.ToString();
        }
        return res;
    }

    public String Permutation(string s, int times){
        // return kth permutation by lexicographical order 
        String res = "";
        //Dictionary<char, int> cnt = new Dictionary<char, int>();
        List<char> order = new List<char>();
        List<int> facto = new List<int>();
        foreach (char x in s){
            order.Add(x);
            facto.Add(0);
        }
        // turn times into factoradic representation
        int n = 1;
        for (int i = s.Length - 1; i > -1; i--){
            facto[i] = times % n;
            times /= n;
            n ++;
            if (times == 0){
                break;
            }
        }
        foreach (int i in facto){
            if (i >= order.Count){
                // times exceed maximum ways of permutation
                s = _Reverse(s);
                return s;
            }
            res += order[i];
            order.Remove(order[i]);
        } 
        return res;
        
    }

    private string _Reverse(string s){
        StringBuilder res = new StringBuilder(s);
        for (int i = 0; i < s.Length / 2; i ++){
            char tmp = res[i];
            res[i] = res[s.Length - i - 1];
            res[s.Length - i - 1] = tmp;
        }
        return res.ToString();
    }
}

class Example {
    static void Main() {
        // for printing unicodes
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        var ex = new Password();
        ex.length = 25;
        ex.symbol = true;
        ex.other = "➌✿";

        var res = ex.Generate();
        Console.WriteLine(res); // Ub➌6yH1a@I0f9'4➌K.;➌➌O✿*h

        res = ex.Permutation(res, 3628799); 
        Console.WriteLine(res); // Ub➌6yH1a@I0f9'4h*✿O➌➌➌;.K
    }
}
