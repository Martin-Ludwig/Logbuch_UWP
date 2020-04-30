namespace Logbuch
{
    class Log
    {
        string _title;
        string _content;
        string _dateTime;

        public int Id
        {
            get;
            set;
        }

        public string Title
        {
            get { return _title; }
            set {
                if (value.Length > 29) {
                    _title = value.Substring(0, 29);
                }
                else
                {
                    _title = value;
                }
                 
                
            }
        }

        public string Content
        {
            get { return _content; }
            set {
                if (value.Length > 199)
                {
                    _content = value.Substring(0, 199);
                }
                else
                {
                    _content = value;
                }

            }
        }

        public string DateTime
        {
            get { return _dateTime; }
            set { 
                if (value.Length > 19)
                {
                    _dateTime = value.Substring(0, 19);

                }
                else
                {
                    _dateTime = value;
                }
            
            }
        }

        public override string ToString()
        {
            return Title;
        }

    }
}
