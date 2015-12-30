using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBGUI
{
    public class IBInitializingException : Exception
    {
        public IBInitializingException() { }
        public IBInitializingException(string message) : base(message) { }
        public IBInitializingException(string message, Exception inner) : base(message, inner) { }
    }

    public class IBLayoutException : Exception
    {
        public IBLayoutException() { }
        public IBLayoutException(string message) : base("画面レイアウトの変更に失敗しました。" + message) { }
        public IBLayoutException(string message,Exception inner) : base("画面レイアウトの変更に失敗しました。" + message, inner) { }
    }

    public class IBDisableCommandException : Exception
    {
        public IBDisableCommandException() { }
        public IBDisableCommandException(string message) : base("コマンドを実行できません。" + message) { }
        public IBDisableCommandException(string message, Exception inner) : base("コマンドを実行できません。" + message, inner) { }
    }
}
