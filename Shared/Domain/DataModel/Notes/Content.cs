using System;

namespace Blink.Shared.Domain.DataModel.Notes
{
    public sealed class Content : IEquatable<Content>
    {
        private string _text;

        public string Text
        {
            get { return String.IsNullOrWhiteSpace(_text) ? String.Empty : _text; }
            set { _text = String.IsNullOrWhiteSpace(value) ? String.Empty : value; }
        }

        public static Content Empty
        {
            get { return new Content(); }
        }

        private Content()
        {
            _text = String.Empty;
        }

        public Content(string text)
        {
            _text = text;
        }

        bool IEquatable<Content>.Equals(Content other)
        {
            return Text.Equals(other.Text);
        }

        new string ToString()
        {
            return Text;
        }
    }
}