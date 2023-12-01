string input = File.ReadAllText("input.txt");

//1
Buffer buffer = new Buffer(4);
int i = 0;

while (!(i > 3 && buffer.StartDetected))
{
    buffer.Push(input[i]);
    i++;
}
Console.WriteLine(i);

//2
buffer = new Buffer(14);
i = 0;

while (!(i > 13 && buffer.StartDetected))
{
    buffer.Push(input[i]);
    i++;
}
Console.WriteLine(i);

class Buffer
{
    private int size;
    private char[] deque;
    private int head = 0;

    public bool StartDetected => deque.Distinct().Count() == size;

    public Buffer(int pSize)
    {
        size = pSize;
        deque = new char[size];
    }

    public void Push(char next)
    {
        deque[head] = next;
        head = (head + 1) % size;
    }
}