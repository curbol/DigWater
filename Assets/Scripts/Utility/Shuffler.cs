using System;

public static class Shuffler
{
    // Fisher-Yates Shuffle
    public static T[] Shuffle<T>(this T[] array, int seed)
    {
        Random random = new Random(seed);

        for (int i = 0; i < array.Length - 1; i++)
        {
            int randomIndex = random.Next(i, array.Length);
            T temp = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = temp;
        }

        return array;
    }
}