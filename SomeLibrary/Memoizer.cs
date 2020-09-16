using System;
using System.Collections.Generic;
using System.Text;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace SomeLibrary
{
   
    /// <summary>
    /// Intercepts method invocation and returns results from memory if args match
    /// </summary>
    public static class Memoizer
    {
        // private field to store memos
        private static Dictionary<string, object> memos = new Dictionary<string, object>();

        [Serializable]
        [MulticastAttributeUsage(MulticastTargets.Method, PersistMetaData = true)]
        public class Memoized : MethodInterceptionAspect
        {
            // intercept the method invocation
            public override void OnInvoke(MethodInterceptionArgs eventArgs)
            {
                // get the arguments that were passed to the method
                object[] args = eventArgs.Arguments.ToArray();

                // start building a key based on the method name
                // because it wouldn't help to return the same value
                // every time "lulu" was passed to any method
                StringBuilder keyBuilder = new StringBuilder(eventArgs.Method.Name);

                // append the hashcode of each arg to the key
                // this limits us to value types (and strings)
                // i need a better way to do this (and preferably
                // a faster one)
                for (int i = 0; i < args.Length; i++)
                    keyBuilder.Append(args[i].GetHashCode());

                string key = keyBuilder.ToString();

                // if the key doesn't exist, invoke the original method
                // passing the original arguments and store the result
                if (!memos.ContainsKey(key))
                    memos[key] = eventArgs.Invoke(eventArgs.Arguments);

                // return the memo
                eventArgs.ReturnValue = memos[key];
            }
        }
    }
}
