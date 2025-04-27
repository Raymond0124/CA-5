using System;

public class AVLNode
{
    public int key;
    public int height;
    public AVLNode left;
    public AVLNode right;

    public AVLNode(int key)
    {
        this.key = key;
        this.height = 1;
        this.left = this.right = null;
    }
}

public class AVLTree
{
    private AVLNode root;

    private int Height(AVLNode node)
    {
        if (node == null)
            return 0;
        return node.height;
    }

    private int GetBalance(AVLNode node)
    {
        if (node == null)
            return 0;
        return Height(node.right) - Height(node.left);
    }

    private AVLNode RightRotate(AVLNode y)
    {
        AVLNode x = y.left;
        AVLNode T2 = x.right;

        x.right = y;
        y.left = T2;

        y.height = Math.Max(Height(y.left), Height(y.right)) + 1;
        x.height = Math.Max(Height(x.left), Height(x.right)) + 1;

        return x;
    }

    private AVLNode LeftRotate(AVLNode x)
    {
        AVLNode y = x.right;
        AVLNode T2 = y.left;

        y.left = x;
        x.right = T2;

        x.height = Math.Max(Height(x.left), Height(x.right)) + 1;
        y.height = Math.Max(Height(y.left), Height(y.right)) + 1;

        return y;
    }

    public void Insert(int key)
    {
        root = InsertRecursive(root, key);
    }

    private AVLNode InsertRecursive(AVLNode node, int key)
    {
        if (node == null)
            return new AVLNode(key);

        if (key < node.key)
            node.left = InsertRecursive(node.left, key);
        else if (key > node.key)
            node.right = InsertRecursive(node.right, key);
        else
            return node; // No se permiten duplicados

        node.height = 1 + Math.Max(Height(node.left), Height(node.right));

        int balance = GetBalance(node);

        // Casos de rotación
        // Izquierda-Izquierda
        if (balance > 1 && key < node.left.key)
            return RightRotate(node);

        // Derecha-Derecha
        if (balance < -1 && key > node.right.key)
            return LeftRotate(node);

        // Izquierda-Derecha
        if (balance > 1 && key > node.left.key)
        {
            node.left = LeftRotate(node.left);
            return RightRotate(node);
        }

        // Derecha-Izquierda
        if (balance < -1 && key < node.right.key)
        {
            node.right = RightRotate(node.right);
            return LeftRotate(node);
        }

        return node;
    }

    public void Delete(int key)
    {
        root = DeleteRecursive(root, key);
    }

    private AVLNode DeleteRecursive(AVLNode node, int key)
    {
        if (node == null)
            return node;

        if (key < node.key)
            node.left = DeleteRecursive(node.left, key);
        else if (key > node.key)
            node.right = DeleteRecursive(node.right, key);
        else
        {
            if ((node.left == null) || (node.right == null))
            {
                AVLNode temp = (node.left != null) ? node.left : node.right;

                if (temp == null)
                {
                    temp = node;
                    node = null;
                }
                else
                {
                    node = temp;
                }
            }
            else
            {
                AVLNode temp = MinValueNode(node.right);
                node.key = temp.key;
                node.right = DeleteRecursive(node.right, temp.key);
            }
        }

        if (node == null)
            return node;

        node.height = 1 + Math.Max(Height(node.left), Height(node.right));

        int balance = GetBalance(node);

        // Casos de rotación
        // Izquierda-Izquierda
        if (balance > 1 && GetBalance(node.left) >= 0)
            return RightRotate(node);

        // Izquierda-Derecha
        if (balance > 1 && GetBalance(node.left) < 0)
        {
            node.left = LeftRotate(node.left);
            return RightRotate(node);
        }

        // Derecha-Derecha
        if (balance < -1 && GetBalance(node.right) <= 0)
            return LeftRotate(node);

        // Derecha-Izquierda
        if (balance < -1 && GetBalance(node.right) > 0)
        {
            node.right = RightRotate(node.right);
            return LeftRotate(node);
        }

        return node;
    }

    private AVLNode MinValueNode(AVLNode node)
    {
        AVLNode current = node;
        while (current.left != null)
            current = current.left;
        return current;
    }

    public void PreOrder()
    {
        PreOrderRecursive(root);
        Console.WriteLine();
    }

    private void PreOrderRecursive(AVLNode node)
    {
        if (node != null)
        {
            Console.Write(node.key + " ");
            PreOrderRecursive(node.left);
            PreOrderRecursive(node.right);
        }
    }
}

// Programa para probar el AVL Tree
class Program
{
    static void Main(string[] args)
    {
        AVLTree tree = new AVLTree();

        /* Insertar nodos */
        tree.Insert(10);
        tree.Insert(20);
        tree.Insert(30);
        tree.Insert(40);
        tree.Insert(50);
        tree.Insert(25);

        Console.WriteLine("Recorrido PreOrder del árbol AVL:");
        tree.PreOrder();

        /* Eliminar un nodo */
        tree.Delete(40);

        Console.WriteLine("\nRecorrido PreOrder después de eliminar 40:");
        tree.PreOrder();
    }
}
