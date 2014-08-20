using UnityEngine;
using System.Collections;


public interface IMoveable
{

	void Move(int x, int y);
}

public interface IBrain
{
    void Think();
}
