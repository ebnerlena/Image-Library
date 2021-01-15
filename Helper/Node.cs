//Lena Ebner - MMTB2019
using System.Drawing;
using System;
public class Node 
{
    public int Value { get; private set; }  //Hashcode of Color
    public Color Color { get; private set; }
    public int AbsoluteFrequency { get; private set; }
    public double RelativeFrequency { get; private set; }
    
    public string code;

    public Node leftChild, rightChild;
    public Node(int value, Color color)
    {
        this.Value = value;
        AbsoluteFrequency = 1;
        this.Color = color;
        code = "";
        leftChild = null;
        rightChild = null;
    }

    public Node (double relativFrequency)
    {
        this.RelativeFrequency = relativFrequency;
        AbsoluteFrequency = 1;
        Value = 0;
        leftChild = null;
        rightChild = null;
    }

    public void IncreaseAbsoluteFrequency()
    {
        this.AbsoluteFrequency++;
    }

    public void CalculateRelativeFrequency(int sum)
    {
        this.RelativeFrequency = (double)AbsoluteFrequency/sum;
    }

    public void SetChildren(Node left, Node right)
    {
        this.leftChild = left;
        this.rightChild = right;
    }
}
