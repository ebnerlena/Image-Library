//Lena Ebner - MMTB2019
using System;
using System.Drawing;

public class Heap
{
	private const int CAPACITY = 100; 
	private int Count; 
	private Node[] arr; 
	private bool isMinHeap;

	public Heap(bool isMin = true)
	{
		this.arr = new Node[CAPACITY];
		this.isMinHeap = isMin;
		this.Count = 0;
	}

	public Heap(Node[] array, bool isMin = true)
	{
		this.arr = new Node[array.Length];
		this.Count =0;
		this.isMinHeap = isMin;
		
		for (int i=0; i<array.Length;i++) {
			this.Add(array[i]);
		}
	}

	public static Heap Setup (Bitmap image)
	{
		Console.WriteLine("Setting up the heap... ");
		Heap minHeap = new Heap(true);
		int sum = image.Width * image.Height;

		for(int x=0; x<image.Width; x++)
        {
			for(int y=0; y<image.Height; y++)
			{
				Color pixelColor = image.GetPixel(x, y);
				int i = 0;
				bool isFound = false;
				while (i < minHeap.Size()){

					if (minHeap.arr[i].Value.Equals(pixelColor.GetHashCode())){
							isFound=true;
							break;
					}
					i++;

				}
				if (isFound){
					minHeap.arr[i].IncreaseAbsoluteFrequency();
					minHeap.arr[i].CalculateRelativeFrequency(sum);
				}
				else {
					Node newNode = new Node(pixelColor.GetHashCode(), pixelColor);
					newNode.CalculateRelativeFrequency(sum);
					minHeap.Add(newNode);
				}
			}
		}
		Console.WriteLine("Setup finished ");
		return minHeap;
	}

	public void PairMinimas()
	{
		
		double sum = this.arr[0].RelativeFrequency + this.arr[1].RelativeFrequency;
		Node parent = new Node(sum);
		parent.SetChildren(arr[0], arr[1]);
		this.Remove();
		this.Remove();
		this.Add(parent);
	}

	public Node GetRootNode()
	{
		if (this.Size() > 1)
			Console.WriteLine("not finished yet: Size: " + this.Size());

		return this.arr[0];
	}

	private bool Compare(int first, int second)
	{
		if (this.isMinHeap) {

            if (arr[first].RelativeFrequency == arr[second].RelativeFrequency)
				return (arr[first].Value < arr[second].Value);

			return (arr[first].RelativeFrequency < arr[second].RelativeFrequency);
		}
		else {

			if (arr[first].RelativeFrequency == arr[second].RelativeFrequency)
				return (arr[first].Value > arr[second].Value);

			return (arr[first].RelativeFrequency > arr[second].RelativeFrequency);
		}
	}

	private void TrickleDown(int parent)
	{		
		int leftChild = parent*2+1;
		int rightChild = parent*2+2;

		if (leftChild == (Count-1) && Compare(leftChild, parent)) {
			Swop(parent, leftChild);
			return;
		}

		if (rightChild == Count && Compare(rightChild, parent)) {
			Swop(parent, rightChild);
			return;
		}

		if (leftChild >= Count || rightChild >= Count){
			return;
		}

		if (Compare(leftChild,rightChild) && Compare(leftChild, parent)) {
			Swop(parent, leftChild);
			TrickleDown(leftChild);
		}
		else if (Compare(rightChild, parent)){
			Swop(parent, rightChild);
			TrickleDown(rightChild);
		}
	}

	private void TrickleUp(int child)
	{
		if (child==0){
			return;
		}
		int parent = (int) Math.Floor((child-1)/2.0);

		if (Compare(child, parent)){
			Swop(child, parent);
			TrickleUp(parent);
		}
	}

	public void Add(Node value)
	{ 
		if (Count == arr.Length-1) {
			DoubleSize();
		}
		arr[Count] = value;
		TrickleUp(Count);
		Count++;
	}

	private void DoubleSize()
	{
		Node[] old = arr;
		arr = new Node[arr.Length * 2];
		Array.Copy(old, 0, arr, 0, Count);
	}

	private void Swop(int parent, int child)
	{
		Node tmp = arr[parent];
		arr[parent] = arr[child];
		arr[child] = tmp;
	}

	public Node Remove()
	{
		Node tmp = arr[0];
		Swop(0, --Count);
		TrickleDown(0);
		return tmp;
	}

	public void Print()
	{
		for (int i = 0; i < Count; i++)
		{
			Console.Write(arr[i] + " ");
		}
	}

	public bool IsEmpty()
	{
		return (Count == 0);
	}

	public int Size()
	{
		return Count;
	}

	public static void HeapSort(Node[] array, bool inc)
	{
		Heap hp = new Heap(array, !inc);
		for (int i = 0; i < array.Length; i++)
		{
			array[array.Length - i - 1] = hp.Remove();
		}
	}
}
