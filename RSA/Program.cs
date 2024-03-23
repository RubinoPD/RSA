using System;
using System.Numerics;

class RSA
{
    static void Main(string[] args)
    {
        BigInteger p = GenerateRandomPrime(100, 1000); // First prime number
        BigInteger q = GenerateRandomPrime(100, 1000); // Second prime number
        BigInteger e = 17; // Public exponent

        // Generate keys
        BigInteger n = p * q;
        BigInteger phi = (p - 1) * (q - 1);
        BigInteger d = CalculatePrivateKey(phi, e);

        // Store public and private keys
        BigInteger[] publicKey = { n, e };
        BigInteger[] privateKey = { n, d };

        while (true)
        {

            Console.WriteLine("RSA Encryption/decryption system\n");
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
                    BigInteger[] encryptedText = Encrypt(plainText, publicKey);
                    Console.WriteLine("Encrypted Text:");
                    foreach (var item in encryptedText)
                    {
                        Console.Write(item + " ");
                    }
                    Console.WriteLine();

                    Console.WriteLine("Do you want to save encrypted text to a file? (yes/no)");
                    string saveOption = Console.ReadLine().ToLower();
                    if (saveOption == "yes")
                    {
                        Console.WriteLine("Enter file name to save:");
                        string fileName = Console.ReadLine();
                        SaveToFile(fileName, encryptedText);
                        Console.WriteLine("Encrypted text saved to file: " + fileName);
                    }
                    break;
                case "2":
                    Console.WriteLine("Choose decryption source:");
                    Console.WriteLine("1. Decrypt from input");
                    Console.WriteLine("2. Decrypt from file");
                    string decryptChoice = Console.ReadLine();

                    if (decryptChoice == "1")
                    {
                        Console.WriteLine("Enter text to decrypt (space-separated numbers):");
                        string encryptedInput = Console.ReadLine();
                        string decryptedText = Decrypt(encryptedInput, privateKey);
                        Console.WriteLine("Decrypted Text: " + decryptedText);
                    }
                    else if (decryptChoice == "2")
                    {
                        Console.WriteLine("Enter filename to decrypt from:");
                        string fileName = Console.ReadLine();
                        string encryptedFromFile = ReadFromFile(fileName);
                        if (encryptedFromFile != null)
                        {
                            string decryptedText = Decrypt(encryptedFromFile, privateKey);
                            Console.WriteLine("Decrypted Text: " + decryptedText);
                        }
                    }
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

    static void SaveToFile(string fileName, BigInteger[] encryptedText)
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            foreach (var item in encryptedText)
            {
                writer.Write(item + " ");
            }
        }
    }

    static string ReadFromFile(string fileName)
    {
        try
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                return reader.ReadToEnd();
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine("Error reading from file: " + ex.Message);
            return null;
        }
    }

    static BigInteger CalculatePrivateKey(BigInteger phi, BigInteger e)
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

    static BigInteger[] Encrypt(string plainText, BigInteger[] publicKey)
    {
        BigInteger n = publicKey[0];
        BigInteger e = publicKey[1];
        BigInteger[] encryptedText = new BigInteger[plainText.Length];

        for (int i = 0; i < plainText.Length; i++)
        {
            BigInteger charValue = plainText[i];
            BigInteger encryptedValue = ModPow(charValue, e, n);
            encryptedText[i] = encryptedValue;
        }

        return encryptedText;
    }

    static string Decrypt(string encryptedInput, BigInteger[] privateKey)
    {
        BigInteger n = privateKey[0];
        BigInteger d = privateKey[1];
        string[] encryptedText = encryptedInput.Split(' ');
        char[] decryptedText = new char[encryptedText.Length];

        for (int i = 0; i < encryptedText.Length; i++)
        {
            int encryptedValue;
            if (!int.TryParse(encryptedText[i], out encryptedValue))
            {
                Console.WriteLine("Invalid input format. Please provide space-separated numbers.");
                return "";
            }

            BigInteger decryptedValue = ModPow(encryptedValue, d, n);
            decryptedText[i] = (char)decryptedValue;
        }

        return new string(decryptedText);
    }

    static BigInteger ModPow(BigInteger value, BigInteger exponent, BigInteger modulus)
    {
        if (exponent == 0)
            return 1;
        if (exponent % 2 == 0)
        {
            BigInteger half = ModPow(value, exponent / 2, modulus);
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

    static BigInteger GenerateRandomPrime(int min, int max)
    {
        Random rand = new Random();
        List<int> primesInRange = GetPrimesInRange(min, max);
        return primesInRange[rand.Next(primesInRange.Count)];
    }

    
}


