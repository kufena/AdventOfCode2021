namespace day24 {

    public class Arg {

        public bool literal {get; set; }
        public long value {get; set; }
        public string acc {get; set; }

        public Arg(string s) {
            long v = 0;
            if (long.TryParse(s,out v)) {
                value = v;
                literal = true;
                acc = "";
            }
            else {
                value = 0;
                literal = false;
                acc = s.Trim();
            }
        }

    }
}