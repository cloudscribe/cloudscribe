// http://stackoverflow.com/questions/66893/tree-data-structure-in-c-sharp
// integrated 2015-07-10
// Modified by Joe Audette
// Last Modified 2015-07-11

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;

namespace cloudscribe.Core.Web.Navigation
{
    public class TreeNode<T>
    {
        private readonly T _value;
        private readonly List<TreeNode<T>> _children = new List<TreeNode<T>>();

        public TreeNode(T value)
        {
            _value = value;
        }

        public TreeNode<T> this[int i]
        {
            get { return _children[i]; }
        }

        //JsonConvert throws an error if an object has a reference to its parent
        [JsonIgnore]
        public TreeNode<T> Parent { get; private set; } = null;

        //JA would like it better if this serialized to Json as "Node" rather than "Value"
        // after testing roudtrip serialization try renaming this to Node then make sure it doesn't break deserialization
        public T Value { get { return _value; } }

        public ReadOnlyCollection<TreeNode<T>> Children
        {
            get { return _children.AsReadOnly(); }
        }

        public TreeNode<T> AddChild(T value)
        {
            var node = new TreeNode<T>(value) { Parent = this };
            _children.Add(node);
            return node;
        }

        public TreeNode<T>[] AddChildren(params T[] values)
        {
            return values.Select(AddChild).ToArray();
        }

        public bool RemoveChild(TreeNode<T> node)
        {
            return _children.Remove(node);
        }

        public void Traverse(Action<T> action)
        {
            action(Value);
            foreach (var child in _children)
                child.Traverse(action);
        }
        

        // added by Joe Audette 2015-07-11
        public TreeNode<T> Find(Func<TreeNode<T>, bool> predicate)
        {
            if(predicate(this)) { return this; }
            foreach(var n in Children)
            {
                var found = n.Find(predicate);
                if (found != null) return found;
            }

            return null;
        }

        

        public IEnumerable<T> Flatten()
        {
            return new[] { Value }.Union(_children.SelectMany(x => x.Flatten()));
        }
    }
}
