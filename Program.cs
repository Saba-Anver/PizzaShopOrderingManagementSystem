using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static PizzaShopOrderingManagementSystem.PizzaShop;

class PizzaShopOrderingManagementSystem
{
    // Defining Data Types

    public class Customer
    {
        public int Age;
        public string Name;
        public string PizzaName;
        public int Quantity;
        public double Bill;

        public Customer() { }

        public Customer(int age, string name, int quantity, string pizzaName, double bill)
        {
            Age = age;
            Name = name;
            PizzaName = pizzaName;
            Quantity = quantity;
            Bill = bill;
        }
    }

    public class TakeAwayCustomer
    {
        public Customer Customer;
        public TakeAwayCustomer Next = null;

        public TakeAwayCustomer(int age, string name, int quantity, string pizzaName, double bill)
        {
            Customer = new Customer(age, name, quantity, pizzaName, bill);
        }
    }

    public class DineInCustomer
    {
        public Customer Customer;
        public DineInCustomer Next = null;

        public DineInCustomer(int age, string name, int quantity, string pizzaName, double bill)
        {
            Customer = new Customer(age, name, quantity, pizzaName, bill);
        }
    }

    public class HomeDeliveryCustomer
    {
        public Customer Customer;
        public string Address;
        public double DeliveryCharges;
        public int DistanceDelivery;
        public HomeDeliveryCustomer Next = null;

        public HomeDeliveryCustomer(int age, string name, int quantity, string pizzaName, double bill, string address, double deliveryCharges, int distanceDelivery)
        {
            Customer = new Customer(age, name, quantity, pizzaName, bill);
            Address = address;
            DeliveryCharges = deliveryCharges;
            DistanceDelivery = distanceDelivery;
        }
    }
    public class PizzaShop
    {
        public string ShopName;
        public string[] Menu;
        public int[] Price;
        public string Address;

        public DineInCustomer NextDineInCustomer { get; set; } = null;
        public HomeDeliveryCustomer NextHomeDeliveryCustomer = null;
        public TakeAwayCustomer NextTakeAwayCustomer { get; set; } = null;
        public void ServeAllOrders()
        {
            while (NextTakeAwayCustomer != null)
            {
                ServeOrderTakeAwayCustomer();
            }
            while (NextDineInCustomer != null)
            {
                ServeOrderDineInCustomer();
            }
            while (NextHomeDeliveryCustomer != null)
            {
                ServeOrderHomeDeliveryCustomer();
            }
        }
        public void ServeOrderTakeAwayCustomer()
        {
            if (this.NextTakeAwayCustomer == null)
            {
                Console.WriteLine("No Take-Away Customer to Serve");
            }
            else
            {
                // Serving the first customer
                TakeAwayCustomer temp = this.NextTakeAwayCustomer;

                // If it has a next element, update the next customer
                if (temp.Next != null)
                {
                    this.NextTakeAwayCustomer = temp.Next;
                }
                else
                {
                    this.NextTakeAwayCustomer = null;
                }

                Console.WriteLine($"Take-Away Customer Served: {temp.Customer.Name}");

                string customerType = "Take-Away";

                // Before deleting the node, update the servedCustomer tree
                root = Insertion(temp.Customer.Age, temp.Customer.Name, temp.Customer.Quantity, temp.Customer.PizzaName, temp.Customer.Bill, customerType, root);

                // Deleting the customer
                temp = null;
            }
        }

        public void ServeOrderDineInCustomer()
        {
            if (this.NextDineInCustomer == null)
            {
                Console.WriteLine("No Dine-In Customer to Serve");
            }
            else
            {
                // Serving the first customer (FIFO)
                DineInCustomer temp = this.NextDineInCustomer;

                // If it has a next element, update the next customer
                if (temp.Next != null)
                {
                    this.NextDineInCustomer = temp.Next;
                }
                else
                {
                    this.NextDineInCustomer = null;
                }

                Console.WriteLine($"Dine-In Customer Served: {temp.Customer.Name}");

                string customerType = "Dine-In";

                // Before deleting the node, update the servedCustomer tree
                root = Insertion(temp.Customer.Age, temp.Customer.Name, temp.Customer.Quantity, temp.Customer.PizzaName, temp.Customer.Bill, customerType, root);

                // Deleting the customer
                temp = null;
            }
        }

        public void ServeOrderHomeDeliveryCustomer()
        {
            if (this.NextHomeDeliveryCustomer == null)
            {
                Console.WriteLine("No Home Delivery Customer to Serve");
            }
            else
            {
                // Serving the first customer (FIFO)
                HomeDeliveryCustomer temp = this.NextHomeDeliveryCustomer; // Get the first customer

                // If it has a next element, update the next customer
                if (temp.Next != null)
                {
                    this.NextHomeDeliveryCustomer = temp.Next; // Move the head to the next customer
                }
                else
                {
                    this.NextHomeDeliveryCustomer = null; // If it was the only customer, set head to null
                }

                Console.WriteLine($"Home Delivery Customer Served: {temp.Customer.Name}");

                string customerType = "Home-Delivery Customer";

                // Before deleting the node, update the servedCustomer tree
                root = Insertion(temp.Customer.Age, temp.Customer.Name, temp.Customer.Quantity, temp.Customer.PizzaName, temp.Customer.Bill, customerType, root);

                // Deleting the customer
                temp = null; // No longer needed, as it's been served
            }
        }

        private void SaveOrderToFile(string orderDetails)
        {
            string filePath = "C:\\Users\\Home Computers\\Documents\\Orders.txt";
            try
            {
                // Append the order details to the file
                using (StreamWriter writer = new StreamWriter(filePath, append: true))
                {
                    writer.WriteLine(orderDetails);
                }
                Console.WriteLine("Order has been saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving the order: {ex.Message}");
            }
        }
        public void PlaceOrderTakeAwayCustomer(int age, string name, string pizzaName, int quantity, double bill)
        {
            // Create a new customer
            TakeAwayCustomer currentTakeAwayCustomer = new TakeAwayCustomer(age, name, quantity, pizzaName, bill);

            // If it's the first customer, insert at the front
            if (this.NextTakeAwayCustomer == null)
            {
                this.NextTakeAwayCustomer = currentTakeAwayCustomer; // Link the first customer
                currentTakeAwayCustomer.Next = null; // Ensure the Next pointer is null
                Console.WriteLine($"Your Order has been Placed, Mr/Mrs {name}, and your order is {pizzaName} with {quantity} quantity. Total bill is {bill}");
            }
            else
            {
                // If there are already customers, find the last customer in the list and insert the new customer
                TakeAwayCustomer temp = this.NextTakeAwayCustomer;
                while (temp.Next != null)
                {
                    temp = temp.Next; // Traverse to the last customer in the list
                }

                // Insert based on age priority (older customer first)
                if (temp.Customer.Age < currentTakeAwayCustomer.Customer.Age)
                {
                    // Insert at the front (higher age priority)
                    TakeAwayCustomer firstCustomer = this.NextTakeAwayCustomer;
                    this.NextTakeAwayCustomer = currentTakeAwayCustomer;
                    currentTakeAwayCustomer.Next = firstCustomer;
                }
                else
                {
                    // Insert at the end of the list
                    temp.Next = currentTakeAwayCustomer;
                    currentTakeAwayCustomer.Next = null; // Ensure the last customer's Next is null
                }

                Console.WriteLine($"Your Order has been Placed, Mr/Mrs {name}, and your order is {pizzaName} with {quantity} quantity. Total bill is {bill}");
            }
        }
        public void PlaceOrderDineInCustomer(int age, string name, string pizzaName, int quantity, double bill)
        {
            // Create a new customer
            DineInCustomer currentDineInCustomer = new DineInCustomer(age, name, quantity, pizzaName, bill);

            // If it's the first customer, insert at the front
            if (this.NextDineInCustomer == null)
            {
                this.NextDineInCustomer = currentDineInCustomer; // Link the first customer
                currentDineInCustomer.Next = null; // Ensure the Next pointer is null
                Console.WriteLine($"\nYour Order has been Placed, Mr/Mrs {name}, and your order is {pizzaName} with {quantity} quantity. Total bill is {bill}");
            }
            else
            {
                // If there are already customers, find the last customer in the list and insert the new customer
                DineInCustomer temp = this.NextDineInCustomer;

                // Traverse to the last customer in the list
                while (temp.Next != null)
                {
                    temp = temp.Next; // Traverse to the last customer in the list
                }

                // Insert based on age priority (older customer first)
                if (temp.Customer.Age < currentDineInCustomer.Customer.Age)
                {
                    // Insert at the front (higher age priority)
                    DineInCustomer firstCustomer = this.NextDineInCustomer;
                    this.NextDineInCustomer = currentDineInCustomer;
                    currentDineInCustomer.Next = firstCustomer; // Link the first customer to the new one
                }
                else
                {
                    // Insert at the end of the list
                    temp.Next = currentDineInCustomer;
                    currentDineInCustomer.Next = null; // Ensure the last customer's Next is null
                }

                Console.WriteLine($"\nYour Order has been Placed, Mr/Mrs {name}, and your order is {pizzaName} with {quantity} quantity. Total bill is {bill}");
            }
        }
        public void PlaceOrderHomeDeliveryCustomer(int age, string name, string pizzaName, int quantity, double bill, string address, int deliveryCharges, int distanceDelivery)
        {
            // Creating a new customer
            HomeDeliveryCustomer currentHomeDeliveryCustomer = new HomeDeliveryCustomer(age, name, quantity, pizzaName, bill, address, deliveryCharges, distanceDelivery);

            if (this.NextHomeDeliveryCustomer == null)
            {
                // If it's the first customer, insert at the front
                this.NextHomeDeliveryCustomer = currentHomeDeliveryCustomer;
                currentHomeDeliveryCustomer.Next = null; // Ensure the Next pointer is null
            }
            else
            {
                // Finding the last node in the list
                HomeDeliveryCustomer temp = this.NextHomeDeliveryCustomer;

                // Traverse to the last customer in the list
                while (temp.Next != null)
                {
                    temp = temp.Next; // Traverse to the last customer in the list
                }

                // Insert based on age priority (older customer first)
                if (temp.Customer.Age < currentHomeDeliveryCustomer.Customer.Age)
                {
                    // Insert at the front (higher age priority)
                    HomeDeliveryCustomer firstCustomer = this.NextHomeDeliveryCustomer;
                    this.NextHomeDeliveryCustomer = currentHomeDeliveryCustomer;
                    currentHomeDeliveryCustomer.Next = firstCustomer; // Link the first customer to the new one
                }
                else
                {
                    // Insert at the end of the list
                    temp.Next = currentHomeDeliveryCustomer;
                    currentHomeDeliveryCustomer.Next = null; // Ensure the last customer's Next is null
                }
            }

            Console.WriteLine($"\nYour Order has been Placed, Mr/Mrs {name}, and your order is {pizzaName} with {quantity} quantity. Total bill is {bill}");
        }


        // Delivery Points Graph using Dijkstra Algorithm
        public static string[] deliveryPoints = { "pizzashop", "north-nazimabad", "gulshan-e-iqbal", "kaechs", "gulistan-e-jauhar", "defense" };

        public static List<List<Tuple<int, int>>> deliveryMap = new List<List<Tuple<int, int>>>
    {
        new List<Tuple<int, int>> { Tuple.Create(1, 2), Tuple.Create(2, 3), Tuple.Create(3, 5), Tuple.Create(5, 4) }, // 0 (pizza shop)
        new List<Tuple<int, int>> { Tuple.Create(0, 2), Tuple.Create(5, 1) }, // 1 (north-nazimabad)
        new List<Tuple<int, int>> { Tuple.Create(0, 3), Tuple.Create(3, 1) }, // 2 (gulshan-e-iqbal)
        new List<Tuple<int, int>> { Tuple.Create(0, 5), Tuple.Create(4, 2), Tuple.Create(5, 2), Tuple.Create(2, 1) }, // 3 (kaechs)
        new List<Tuple<int, int>> { Tuple.Create(3, 2), Tuple.Create(5, 2) }, // 4 (Johar Town)
        new List<Tuple<int, int>> { Tuple.Create(0, 4), Tuple.Create(1, 1), Tuple.Create(3, 2), Tuple.Create(4, 2) } // 5 (defense)
    };
        const int Infinity = int.MaxValue;

        public List<int> Dijkstra(int sourceNode)
        {
            List<int> distance = new List<int>(new int[6]);
            for (int i = 0; i < 6; i++) distance[i] = Infinity;

            SortedSet<Tuple<int, int>> s = new SortedSet<Tuple<int, int>>();
            distance[sourceNode] = 0;
            s.Add(Tuple.Create(0, sourceNode));

            while (s.Count > 0)
            {
                var top = s.Min;
                int u = top.Item2;

                s.Remove(top);

                foreach (var child in deliveryMap[u])
                {
                    int childVertex = child.Item1;
                    int childWeight = child.Item2;

                    if (distance[u] + childWeight < distance[childVertex])
                    {
                        distance[childVertex] = distance[u] + childWeight;
                        s.Add(Tuple.Create(distance[childVertex], childVertex));
                    }
                }
            }

            return distance;
        }



        public void DisplayAllOrdersTakeAwayCustomers(StreamWriter writer = null)
        {
            if (NextTakeAwayCustomer == null)
            {
                string noOrderMessage = "There is no Order for Take-Away Customer till yet";
                Console.WriteLine(noOrderMessage);
                writer?.WriteLine(noOrderMessage);
            }
            else
            {
                TakeAwayCustomer traversal = NextTakeAwayCustomer;
                while (traversal != null)
                {
                    string divider = "-----------------------------------------------------";
                    string customerDetails = $"Take-Away Customer : {traversal.Customer.Name}\n" +
                                             $"Age : {traversal.Customer.Age}\n" +
                                             $"Pizza Name : {traversal.Customer.PizzaName}\n" +
                                             $"Quantity : {traversal.Customer.Quantity}\n" +
                                             $"Bill : {traversal.Customer.Bill} RS/_";

                    Console.WriteLine(divider);
                    Console.WriteLine(customerDetails);
                    Console.WriteLine(divider);

                    writer?.WriteLine(divider);
                    writer?.WriteLine(customerDetails);
                    writer?.WriteLine(divider);

                    traversal = traversal.Next;
                }
            }
        }

        public void DisplayAllOrdersHomeDeliveryCustomers(StreamWriter writer = null)
        {
            if (NextHomeDeliveryCustomer == null)
            {
                string noOrderMessage = "There is no Order for Home Delivery Customer till yet";
                Console.WriteLine(noOrderMessage);
                writer?.WriteLine(noOrderMessage);
            }
            else
            {
                HomeDeliveryCustomer traversal = NextHomeDeliveryCustomer;
                while (traversal != null)
                {
                    string divider = "-----------------------------------------------------";
                    string customerDetails = $"Home Delivery Customer : {traversal.Customer.Name}\n" +
                                             $"Age : {traversal.Customer.Age}\n" +
                                             $"Pizza Name : {traversal.Customer.PizzaName}\n" +
                                             $"Quantity : {traversal.Customer.Quantity}\n" +
                                             $"Delivery Distance : {traversal.DistanceDelivery} KM\n" +
                                             $"Delivery Charges : {traversal.DeliveryCharges}\n" +
                                             $"Bill : {traversal.Customer.Bill} RS/_\n" +
                                             $"Delivery Address : {traversal.Address}";

                    Console.WriteLine(divider);
                    Console.WriteLine(customerDetails);
                    Console.WriteLine(divider);

                    writer?.WriteLine(divider);
                    writer?.WriteLine(customerDetails);
                    writer?.WriteLine(divider);

                    traversal = traversal.Next;
                }
            }
        }

        public void DisplayAllOrdersDineInCustomers(StreamWriter writer = null)
        {
            if (NextDineInCustomer == null)
            {
                string noOrderMessage = "There is no Order for Dine-In Customer till yet";
                Console.WriteLine(noOrderMessage);
                writer?.WriteLine(noOrderMessage);
            }
            else
            {
                DineInCustomer traversal = NextDineInCustomer;
                while (traversal != null)
                {
                    string divider = "-----------------------------------------------------";
                    string customerDetails = $"Walking Customer : {traversal.Customer.Name}\n" +
                                             $"Age : {traversal.Customer.Age}\n" +
                                             $"Pizza Name : {traversal.Customer.PizzaName}\n" +
                                             $"Quantity : {traversal.Customer.Quantity}\n" +
                                             $"Bill : {traversal.Customer.Bill} RS/_";

                    Console.WriteLine(divider);
                    Console.WriteLine(customerDetails);
                    Console.WriteLine(divider);

                    writer?.WriteLine(divider);
                    writer?.WriteLine(customerDetails);
                    writer?.WriteLine(divider);

                    traversal = traversal.Next;
                }
            }
        }
        public void DisplayAllOrders()
        {
            string filePath = "C:\\Users\\Home Computers\\Documents\\Orders.txt";

            using (StreamWriter writer = new StreamWriter(filePath, append: false))
            {
                writer.WriteLine("The Take-Away Customers Are:");
                Console.WriteLine("The Take-Away Customers Are :");
                DisplayAllOrdersTakeAwayCustomers(writer);

                writer.WriteLine("The Dine-IN Customers Are:");
                Console.WriteLine("The Dine-IN Customers Are :");
                DisplayAllOrdersDineInCustomers(writer);

                writer.WriteLine("The Home Delivery Customers Are:");
                Console.WriteLine("The Home Delivery Customers Are :");
                DisplayAllOrdersHomeDeliveryCustomers(writer);
            }
        }
        public void TotalBillOfPendingOrders()
        {
            double takeAway = 0, dineIn = 0, homeDelivery = 0, total = 0;

            TakeAwayCustomer p = NextTakeAwayCustomer;
            while (p != null)
            {
                takeAway += p.Customer.Bill;
                p = p.Next;
            }

            DineInCustomer q = NextDineInCustomer;
            while (q != null)
            {
                dineIn += q.Customer.Bill;
                q = q.Next;
            }

            HomeDeliveryCustomer r = NextHomeDeliveryCustomer;
            while (r != null)
            {
                homeDelivery += r.Customer.Bill;
                r = r.Next;
            }

            total = takeAway + dineIn + homeDelivery;

            Console.WriteLine($"The total bill of pending orders for Take-Away customers are : {takeAway} RS/_");
            Console.WriteLine($"The total bill of pending orders for Dine-In customers are : {dineIn} RS/_");
            Console.WriteLine($"The total bill of pending orders for Delivery customers are : {homeDelivery} RS/_");
            Console.WriteLine($"The Total orders pending are : {total} RS/_");
        }
        public double TotalEarnings(ServedCustomer root)
        {
            double servedTotal = 0;
            if (root != null)
            {
                // Recursively add earnings from the left subtree
                servedTotal += TotalEarnings(root.Left);

                // Add the earnings from the current node
                servedTotal += root.Bill;

                // Recursively add earnings from the right subtree
                servedTotal += TotalEarnings(root.Right);
            }
            return servedTotal;
        }




        // Globally declaring the pizza Shop's pointer
        public static PizzaShop myPizzaShop = null;

        // Globally Declaring the Current Customer's Pointers for all three Types
        public static TakeAwayCustomer currentTakeAwayCustomer = null;
        public static DineInCustomer currentDineInCustomer = null;
        public static HomeDeliveryCustomer currentHomeDeliveryCustomer = null;

        // Globally declaring the variables for the total of all the orders in queue!
        public static double total, takeAway, dineIn, homeDelivery;

        // Globally declaring the variables for the total of all the orders served!
        public static double servedTotal;

        // In case of Serving, to keep the record of Customers Served, implementing AVL Tree for efficient Search
        // to search the record of Customers by Name
        // It can also Display all the customers Served


        public class ServedCustomer
        {
            public int Age;
            public string Name;
            public int Quantity;
            public string PizzaName;
            public double Bill;
            public string CustomerType;
            public ServedCustomer Left;
            public ServedCustomer Right;

            // Constructor
            public ServedCustomer(int age, string name, int quantity, string pizzaName, double bill, string customerType)
            {
                // setting customers details
                Age = age;
                Name = name;
                Quantity = quantity;
                PizzaName = pizzaName;
                Bill = bill;
                CustomerType = customerType;

                // child to NULL
                Left = null;
                Right = null;
            }
        }

        public static ServedCustomer root = null; // Global pointer for the root of AVLTree

        // isEmpty or not
        public static int IsEmpty(ServedCustomer root)
        {
            return root == null ? 1 : 0;
        }

        // Display Customer's Details
        public static void Display(ServedCustomer root)
        {
            Console.WriteLine("Name :" + root.Name);
            Console.WriteLine("Age  :" + root.Age);
            Console.WriteLine("Pizza :" + root.PizzaName);
            Console.WriteLine("Quantity : " + root.Quantity);
            Console.WriteLine("Bill : " + root.Bill);
            Console.WriteLine("Customer Type: " + root.CustomerType);
        }

        // Method to display all served orders in the AVL tree
        public static void DisplayAllServedOrders(ServedCustomer root) // Changed return type to void
        {
            if (root != null)
            {
                DisplayAllServedOrders(root.Left); // No reassignment 
                Display(root); // Display current node's data 
                DisplayAllServedOrders(root.Right); // No reassignment 
            }
        }

        // Method to find the height of the servedCustomer tree
        public static int Height(ServedCustomer root)
        {
            if (root == null)
                return 0;

            return Math.Max(Height(root.Left), Height(root.Right)) + 1;
        }

        // Method to calculate the balance factor for each ServedCustomer node
        public static int BalanceFactor(ServedCustomer root)
        {
            if (root == null)
                return 0;

            return Height(root.Left) - Height(root.Right);
        }

        // Helper method to find the maximum of two integers
        public static int Max(int a, int b)
        {
            return (a > b) ? a : b;
        }

        // Method to search for a servedCustomer in the tree
        public static ServedCustomer Search(ServedCustomer root, string keyName)
        {
            if (root == null)
            {
                return null;
            }
            else if (root.Name == keyName)
            {
                return root;
            }
            else if (root.Name.CompareTo(keyName) < 0)
            {
                return Search(root.Right, keyName);
            }
            else
            {
                return Search(root.Left, keyName);
            }
        }

        // Method to find the maximum node in the servedCustomer tree
        public static ServedCustomer MaxServedCustomer(ServedCustomer root)
        {
            ServedCustomer p = root;
            ServedCustomer temp = null;

            while (p != null)
            {
                temp = p;
                p = p.Right;
            }

            return temp;
        }

        // LL Rotation for balancing the tree
        public static ServedCustomer LLRotation(ServedCustomer root)
        {
            ServedCustomer x = root.Left;
            ServedCustomer temp = x.Right;

            // Performing rotation
            x.Right = root;
            root.Left = temp;

            // Updating the root
            root = x;
            return x;
        }

        // RR Rotation for balancing the tree
        public static ServedCustomer RRRotation(ServedCustomer root)
        {
            ServedCustomer x = root.Right;
            ServedCustomer temp = x.Left;

            // Performing rotation
            x.Left = root;
            root.Right = temp;

            // Updating the root
            root = x;
            return x;
        }

        // LR Rotation for balancing the tree
        public static ServedCustomer LRRotation(ServedCustomer root)
        {
            root.Left = RRRotation(root.Left);
            return LLRotation(root);
        }

        // RL Rotation for balancing the tree
        public static ServedCustomer RLRotation(ServedCustomer root)
        {
            root.Right = LLRotation(root.Right);
            return RRRotation(root);
        }

        // Insertion in the servedCustomer tree
        public static ServedCustomer Insertion(int age, string name, int quantity, string pizzaName, double bill, string customerType, ServedCustomer root)
        {
            ServedCustomer newNode = new ServedCustomer(age, name, quantity, pizzaName, bill, customerType);

            if (root == null)
            {
                root = newNode;
            }
            else if (root.Name.CompareTo(newNode.Name) > 0)
            {
                root.Left = Insertion(age, name, quantity, pizzaName, bill, customerType, root.Left);
            }
            else if (root.Name.CompareTo(newNode.Name) < 0)
            {
                root.Right = Insertion(age, name, quantity, pizzaName, bill, customerType, root.Right);
            }
            else
            {
                Console.WriteLine("No duplicate values are allowed");
                return root;
            }

            int bf = BalanceFactor(root);

            if (bf == 2)
            {
                // LL Rotation
                if (root.Left.Name.CompareTo(newNode.Name) > 0)
                {
                    return LLRotation(root);
                }

                // LR Rotation
                if (root.Left.Name.CompareTo(newNode.Name) < 0)
                {
                    return LRRotation(root);
                }
            }
            else if (bf == -2)
            {
                // RR Rotation
                if (root.Right.Name.CompareTo(newNode.Name) < 0)
                {
                    return RRRotation(root);
                }

                // RL Rotation
                if (root.Right.Name.CompareTo(newNode.Name) > 0)
                {
                    return RLRotation(root);
                }
            }

            return root; // Return root if no rotation is needed
        }

        // Method to delete a node in the AVL tree
        public static ServedCustomer DeleteNode(ServedCustomer root, string key)
        {
            if (root == null)
                return root;

            if (key.CompareTo(root.Name) < 0)
                root.Left = DeleteNode(root.Left, key);
            else if (key.CompareTo(root.Name) > 0)
                root.Right = DeleteNode(root.Right, key);
            else
            {
                // If the node to delete has one child or no children
                if (root.Left == null || root.Right == null)
                {
                    ServedCustomer temp = root.Left != null ? root.Left : root.Right;

                    if (temp == null)
                    {
                        temp = root;
                        root = null;
                    }
                    else
                    {
                        root = temp;
                    }
                }
                else
                {
                    // If the node to delete has two children
                    ServedCustomer temp = MaxServedCustomer(root.Right);
                    root.Name = temp.Name;
                    root.Right = DeleteNode(root.Right, temp.Name);
                }
            }

            if (root == null)
                return root;

            // Getting the balance factor
            int balance = BalanceFactor(root);

            // Rotation cases
            // LEFT LEFT case
            if (balance > 1 && key.CompareTo(root.Left.Name) < 0)
                return LLRotation(root);

            // LEFT RIGHT case
            if (balance > 1 && key.CompareTo(root.Left.Name) > 0)
            {
                root.Left = LLRotation(root.Left);
                return LRRotation(root);
            }

            // RIGHT RIGHT case
            if (balance < -1 && key.CompareTo(root.Right.Name) > 0)
                return RRRotation(root);

            // RIGHT LEFT case
            if (balance < -1 && key.CompareTo(root.Right.Name) < 0)
                return RLRotation(root);

            return root;
        }

        // Method to delete all served customers in the AVL tree
        public static void DeleteAllServedCustomers(ref ServedCustomer root)
        {
            while (root != null)
            {
                root = DeleteNode(root, root.Name);
            }

            Console.WriteLine("The Served Customer's List is Cleared");
        }
        static void Main()
        {
            // Create instance of PizzaShop
            PizzaShop myPizzaShop = new PizzaShop();

            // Set PizzaShop properties
            myPizzaShop.ShopName = "The Pizza Delight";
            myPizzaShop.Address = "Liberty Chowk, Karachi";
            myPizzaShop.Menu = new string[11] { "", "chickenTikka", "arabicRanch", "chickenFajita", "cheeseLover", "chickenSupreme", "allveggie", "garlicWest", "BeefBold", "phantom", "mexicanDelight" };
            myPizzaShop.Price = new int[11] { 0, 2000, 2500, 2400, 2200, 2700, 2000, 2100, 3000, 3000, 2800 };

            string option;
            // Main program loop
            do
            {
                Console.WriteLine("-------------------------------------------------------------------------");
                Console.WriteLine("------------------------------" + myPizzaShop.ShopName + "----------------------------");
                Console.WriteLine("-------------------------------------------------------------------------");
                Console.WriteLine("Located at " + myPizzaShop.Address);
                Console.WriteLine("Our Menu is as follows: ");
                for (int i = 1; i <= 10; i++)
                {
                    Console.WriteLine(i + ". " + myPizzaShop.Menu[i] + " - " + myPizzaShop.Price[i]);
                }

                Console.WriteLine("\nPlease select your role:");
                Console.WriteLine("1. Customer");
                Console.WriteLine("2. Pizza Staff");
                int roleChoice = int.Parse(Console.ReadLine());

                // Customer Role Options
                if (roleChoice == 1)
                {
                    Console.WriteLine("\nWelcome Customer! Please choose an option:");
                    Console.WriteLine("C1. Place order for Take-Away Customer");
                    Console.WriteLine("C2. Place order for Home Delivery Customer");
                    Console.WriteLine("C3. Place order for Dine-In Customer");
                    Console.WriteLine("0. EXIT");
                }
                // Pizza Staff Role Options
                else if (roleChoice == 2)
                {
                    Console.WriteLine("\nWelcome Pizza Staff! Please choose an option:");
                    Console.WriteLine("S1. Serve order for Take-Away Customer");
                    Console.WriteLine("S2. Serve order for Home Delivery Customer");
                    Console.WriteLine("S3. Serve order for Dine-In Customer");
                    Console.WriteLine("S4. Serve All Orders");
                    Console.WriteLine("S5. Display all orders of Take-Away Customers");
                    Console.WriteLine("S6. Display all orders of Home Delivery Customers");
                    Console.WriteLine("S7. Display all orders of Dine-In Customers");
                    Console.WriteLine("S8. Display all Pending Orders of all Customers");
                    Console.WriteLine("S9. Display the total Earnings of Served Orders");
                    Console.WriteLine("S10. Search served customers");
                    Console.WriteLine("S11. Delete served customers");
                    Console.WriteLine("S12. Display total bill of Pending Orders");
                    Console.WriteLine("S13. Display All Orders");
                    Console.WriteLine("0. EXIT");
                }
                else
                {
                    Console.WriteLine("Invalid role selection. Please restart.");
                    break; // Exit the loop if invalid role is selected
                }

                Console.Write("\nEnter your choice: ");
                option = Console.ReadLine();

                // Taking input for Customer Details
                int age, quantity, pizzaIndex;
                double bill;
                string address, name;

                option = option.ToUpper();
                switch (option)
                {
                    case "C1":
                        // Placing order for take away customer
                        Console.Write("Enter the name of the customer: ");
                        name = Console.ReadLine();
                        Console.Write("Enter the age of the customer: ");
                        age = int.Parse(Console.ReadLine());
                        if (age < 12)
                        {
                            Console.WriteLine("Sorry, orders cannot be placed for customers under the age of 12.");
                            goto case "0"; // Exit case
                        }
                        Console.Write("Enter the quantity of the pizza: ");
                        quantity = int.Parse(Console.ReadLine());
                        Console.Write("Enter the option for the pizza: ");
                        pizzaIndex = int.Parse(Console.ReadLine());
                        bill = quantity * myPizzaShop.Price[pizzaIndex];
                        myPizzaShop.PlaceOrderTakeAwayCustomer(age, name, myPizzaShop.Menu[pizzaIndex], quantity, bill);

                        myPizzaShop.SaveOrderToFile($"TakeAway | {name} | Age: {age} | Pizza: {myPizzaShop.Menu[pizzaIndex]} | Quantity: {quantity} | Bill: {bill} RS/_");
                        break;


                    case "C2":
                        // Placing order for home delivery customer
                        Console.Write("Enter the name of the customer: ");
                        name = Console.ReadLine();
                        Console.Write("Enter the age of the customer: ");
                        age = int.Parse(Console.ReadLine());
                        if (age < 12)
                        {
                            Console.WriteLine("Sorry, orders cannot be placed for customers under the age of 12.");
                            goto case "0"; // Exit case
                        }
                        Console.Write("Enter the quantity of the pizza: ");
                        quantity = int.Parse(Console.ReadLine());
                        Console.Write("Enter the option for the pizza: ");
                        pizzaIndex = int.Parse(Console.ReadLine());
                        Console.Write("Enter the delivery address (choose one from the list): ");
                        Console.WriteLine("0. pizzashop");
                        Console.WriteLine("1. north-nazimabad");
                        Console.WriteLine("2. gulshan-e-iqbal");
                        Console.WriteLine("3. kaechs");
                        Console.WriteLine("4. gulistan-e-jauhar");
                        Console.WriteLine("5. defense");
                        address = Console.ReadLine().ToLower();

                        int deliveryIndex = Array.IndexOf(deliveryPoints, address);
                        if (deliveryIndex == -1)
                        {
                            Console.WriteLine("Invalid address. Please choose a valid location.");
                            break;
                        }

                        var distances = myPizzaShop.Dijkstra(0);
                        int deliveryDistance = distances[deliveryIndex];
                        int deliveryCharges = deliveryDistance * 50;
                        bill = (quantity * myPizzaShop.Price[pizzaIndex]) + deliveryCharges;
                        myPizzaShop.PlaceOrderHomeDeliveryCustomer(age, name, myPizzaShop.Menu[pizzaIndex], quantity, bill, address, deliveryCharges, deliveryDistance);
                        myPizzaShop.SaveOrderToFile($"HomeDelivery | {name} | Age: {age} | Pizza: {myPizzaShop.Menu[pizzaIndex]} | Quantity: {quantity} | Bill: {bill} RS/_ | Address: {address} | Delivery Charges: {deliveryCharges} | Distance: {deliveryDistance} km");
                        break;

                    case "C3":
                        // Placing order for dine-in customer
                        Console.Write("Enter the name of the customer: ");
                        name = Console.ReadLine();
                        Console.Write("Enter the age of the customer: ");
                        age = int.Parse(Console.ReadLine());
                        if (age < 12)
                        {
                            Console.WriteLine("Sorry, orders cannot be placed for customers under the age of 12.");
                            goto case "0"; // Exit case
                        }
                        Console.Write("Enter the quantity of the pizza: ");
                        quantity = int.Parse(Console.ReadLine());
                        Console.Write("Enter the option for the pizza: ");
                        pizzaIndex = int.Parse(Console.ReadLine());
                        bill = quantity * myPizzaShop.Price[pizzaIndex];
                        myPizzaShop.PlaceOrderDineInCustomer(age, name, myPizzaShop.Menu[pizzaIndex], quantity, bill);
                        myPizzaShop.SaveOrderToFile($"DineIn | {name} | Age: {age} | Pizza: {myPizzaShop.Menu[pizzaIndex]} | Quantity: {quantity} | Bill: {bill} RS/_");
                        break;

                    case "S1":
                        // Serving order for take-away customer
                        myPizzaShop.ServeOrderTakeAwayCustomer();
                        break;

                    case "S2":
                        // Serving order for home delivery customer
                        myPizzaShop.ServeOrderHomeDeliveryCustomer();
                        break;

                    case "S3":
                        // Serving order for dine-in customer
                        myPizzaShop.ServeOrderDineInCustomer();
                        break;

                    case "S4":
                        // Serving all orders
                        myPizzaShop.ServeAllOrders();
                        break;

                    case "S5":
                        // Display all orders of Take-away customers
                        myPizzaShop.DisplayAllOrdersTakeAwayCustomers();
                        break;

                    case "S6":
                        // Display all orders of Home Delivery customers
                        myPizzaShop.DisplayAllOrdersHomeDeliveryCustomers();
                        break;

                    case "S7":
                        // Display all orders of Dine-In customers
                        myPizzaShop.DisplayAllOrdersDineInCustomers();
                        break;

                    case "S8":
                        // Display all orders of all customers
                        myPizzaShop.DisplayAllOrders();
                        Console.WriteLine("All orders have been saved to 'Orders.txt'.");
                        break;

                    case "S9":
                        // Display total earnings from served orders
                        Console.WriteLine("Total Earnings from Served Orders");
                        Console.WriteLine(myPizzaShop.TotalEarnings(root));
                        break;

                    case "S10":
                        // Searching served orders
                        Console.WriteLine("Enter the name of the customer you want to search: ");
                        name = Console.ReadLine();
                        ServedCustomer searchedCustomer = Search(root, name);
                        if (searchedCustomer == null)
                        {
                            Console.WriteLine("No such customer was served.");
                        }
                        else
                        {
                            Console.WriteLine("-----------------------------------------------------");
                            Console.WriteLine($"Served Customer: {searchedCustomer.Name}");
                            Console.WriteLine($"Age: {searchedCustomer.Age}");
                            Console.WriteLine($"Pizza Name: {searchedCustomer.PizzaName}");
                            Console.WriteLine($"Quantity: {searchedCustomer.Quantity}");
                            Console.WriteLine($"Bill: {searchedCustomer.Bill} RS/_");
                            Console.WriteLine($"Customer Type: {searchedCustomer.CustomerType}");
                            Console.WriteLine("-----------------------------------------------------");
                        }
                        break;

                    case "S11":
                        // Deleting served customers from the AVL tree
                        DeleteAllServedCustomers(ref root);
                        break;
                    case "S12":
                        // Display total bill of pending orders
                        Console.WriteLine("Total Bill of Pending Orders");
                        myPizzaShop.TotalBillOfPendingOrders();
                        break;
                    case "S13":
                        try
                        {
                            string filePath = "C:\\Users\\Home Computers\\Documents\\Orders.txt";

                            // Check if the file exists
                            if (File.Exists(filePath))
                            {
                                // Read all orders from the file
                                string[] orders = File.ReadAllLines(filePath);

                                // Display each order
                                Console.WriteLine("\n--- All Orders ---");
                                foreach (string order in orders)
                                {
                                    Console.WriteLine(order);
                                }
                                Console.WriteLine("-------------------\n");
                            }
                            else
                            {
                                Console.WriteLine("No orders have been placed yet. The order history file does not exist.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An error occurred while reading the order file: {ex.Message}");
                        }
                        break;


                    case "0":
                        Console.WriteLine("Pizza Order Management System -- Terminated");
                        Console.WriteLine("Thank you for using our services!");
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            } while (option != "0");
        }
    }
}