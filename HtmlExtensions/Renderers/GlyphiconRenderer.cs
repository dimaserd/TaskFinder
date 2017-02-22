namespace HtmlExtensions.Renderers
{
    public class GlyphiconRenderer
    {
        public string Trash
        {
            get { return "<span class='glyphicon glyphicon-trash'></span>"; }
        }

        public string MinusSign
        {
            get { return "<span class='glyphicon glyphicon-minus'></span>"; }
        }

        public string ListAlt
        {
            get { return "<span class='glyphicon glyphicon-list-alt'></span>"; }
        }

        public string Pencil
        {
            get { return "<span class='glyphicon glyphicon-pencil'></span>"; }
        }

        public string Send
        {
            get { return "<span class='glyphicon glyphicon-send'></span>"; }
        }

        public string Gift
        {
            get { return "<span class='glyphicon glyphicon-gift'></span>"; }
        }

        public string PiggyBank
        {
            get { return "<span class='glyphicon glyphicon-piggy-bank'></span>"; }
        }

        public string Login
        {
            get { return "<span class='glyphicon glyphicon-log-in'></span>"; }
        }

        public string Envelope
        {
            get { return "<span class='glyphicon glyphicon-envelope'></span>"; }
        }


        public string User
        {
            get { return "<span class='glyphicon glyphicon-user'></span>"; }
        }

        public string Star
        {
            get { return "<span class='glyphicon glyphicon-star'></span>"; }
        }

        public string Save
        {
            get { return "<span class='glyphicon glyphicon-save'></span>"; }
        }

        public string Briefcase
        {
            get { return "<span class='glyphicon glyphicon-briefcase'></span>"; }
        }

        public string PlusSign
        {
            get { return "<span class='glyphicon glyphicon-plus'> </span>"; }
        }

        public string Count(int count)
        {
            return $"<span class='badge'>{count}</span>";
        }

        public string Configuration
        {
            get { return $"<span class='glyphicon glyphicon-cog'></span>"; }
        }

        public string Ok
        {
            get { return $"<span class='glyphicon glyphicon-ok'></span>"; }
        }

        public string Refresh
        {
            get { return $"<span class='glyphicon glyphicon-refresh'></span>"; }
        }
    }
}
