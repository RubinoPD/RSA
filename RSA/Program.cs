using System;

class RSA
{
    static void Main(string[] args)
    {
        int p = GenerateRandomPrime(50, 100); // First prime number
        int q = GenerateRandomPrime(50, 100); // Second prime number
        int e = 17; // Public exponent

        // Generate keys
        int n = p * q;
        int phi = (p - 1) * (q - 1);
        int d = CalculatePrivateKey(phi, e);

        // Store public and private keys
        int[] publicKey = { n, e };
        int[] privateKey = { n, d };

        while (true)
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Encrypt");
            Console.WriteLine("2. Decrypt");
            Console.WriteLine("3. Exit");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Enter text to encrypt:");
                    string plainText = Console.ReadLine();
                    int[] encryptedText = Encrypt(plainText, publicKey);
                    Console.WriteLine("Encrypted Text:");
                    foreach (var item in encryptedText)
                    {
                        Console.Write(item + " ");
                    }
                    Console.WriteLine();
                    break;
                case "2":
                    Console.WriteLine("Enter text to decrypt (space-separated numbers):");
                    string encryptedInput = Console.ReadLine();
                    string decryptedText = Decrypt(encryptedInput, privateKey);
                    Console.WriteLine("Decrypted Text: " + decryptedText);
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
                    break;
            }
        }
    }

    static int CalculatePrivateKey(int phi, int e)
    {
        int d = 0;
        while (true)
        {
            d++;
            if ((d * e) % phi == 1)
            {
                break;
            }
        }
        return d;
    }

    static int[] Encrypt(string plainText, int[] publicKey)
    {
        int n = publicKey[0];
        int e = publicKey[1];
        int[] encryptedText = new int[plainText.Length];

        for (int i = 0; i < plainText.Length; i++)
        {
            int charValue = plainText[i];
            int encryptedValue = ModPow(charValue, e, n);
            encryptedText[i] = encryptedValue;
        }

        return encryptedText;
    }

    static string Decrypt(string encryptedInput, int[] privateKey)
    {
        int n = privateKey[0];
        int d = privateKey[1];
        string[] encryptedText = encryptedInput.Split(' ');
        char[] decryptedText = new char[encryptedText.Length];

        for (int i = 0; i < encryptedText.Length; i++)
        {
            int encryptedValue = int.Parse(encryptedText[i]);
            int decryptedValue = ModPow(encryptedValue, d, n);
            decryptedText[i] = (char)decryptedValue;
        }

        return new string(decryptedText);
    }

    static int ModPow(int value, int exponent, int modulus)
    {
        if (exponent == 0)
            return 1;
        if (exponent % 2 == 0)
        {
            int half = ModPow(value, exponent / 2, modulus);
            return (half * half) % modulus;
        }
        else
        {
            return (value * ModPow(value, exponent - 1, modulus)) % modulus;
        }
    }

    static bool IsPrime(int num)
    {
        if (num <= 1)
            return false;
        if (num <= 3)
            return true;

        if (num % 2 == 0 || num % 3 == 0)
            return false;

        for (int i = 5; i * i <= num; i += 6)
        {
            if (num % i == 0 || num % (i + 2) == 0)
                return false;
        }

        return true;
    }

    static List<int> GetPrimesInRange(int start, int end)
    {
        List<int> primes = new List<int>();

        for (int i = start; i <= end; i++)
        {
            if (IsPrime(i))
                primes.Add(i);
        }

        return primes;
    }

    static int GenerateRandomPrime(int min, int max)
    {
        Random rand = new Random();
        List<int> primesInRange = GetPrimesInRange(min, max);
        return primesInRange[rand.Next(primesInRange.Count)];
    }
}


