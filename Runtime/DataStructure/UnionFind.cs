using System.Collections.Generic;
using System.Linq;

namespace Corelib.Utils
{
    public class UnionFind
    {
        private List<int> parent, rank;
        public int size;

        public UnionFind(int size)
        {
            this.size = size;
            parent = Enumerable.Range(0, size).ToList();
            rank = new List<int>().Resize(size);
        }

        public int Find(int x)
        {
            int root = x;
            while (root != parent[root])
                root = parent[root];

            while (x != root)
            {
                int next = parent[x];
                parent[x] = root;
                x = next;
            }
            return root;
        }

        public bool Merge(int a, int b)
        {
            (a, b) = (Find(a), Find(b));
            if (a == b) return false;
            if (rank[a] < rank[b])
                (a, b) = (b, a);
            parent[b] = a;
            rank[a] += rank[a] == rank[b] ? 1 : 0;

            return true;
        }

        public bool Join(int a, int b) => Find(a) == Find(b);
    }

    public class RollbackUnionFind
    {

    }
}