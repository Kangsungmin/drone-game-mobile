using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneModel {

	private int ID;
	private string Title;
	private int Price;
    public List<Pair<Part, int>> Recipe = new List<Pair<Part, int>>();//드론에 사용되는 재료 부품과 각 부품 카운트를 저장하는 리스트

    public DroneModel (int id, string title, int price)
	{
		this.ID = id;
		this.Title = title;
		this.Price = price;
        //===========DB구현시 삭제 요망==========
        //삭제 후, Loadding씬에서 해당 코드를 구현해야 한다.
        switch(id)
        {
            case 0:
                break;
            case 1:
                Recipe.Add(new Pair<Part, int>(new Part(6), 2));
                Recipe.Add(new Pair<Part, int>(new Part(7), 2));
                Recipe.Add(new Pair<Part, int>(new Part(8), 1));
                Recipe.Add(new Pair<Part, int>(new Part(9), 1));
                break;
            case 2:
                Recipe.Add(new Pair<Part, int>(new Part(6), 4));
                Recipe.Add(new Pair<Part, int>(new Part(7), 4));
                Recipe.Add(new Pair<Part, int>(new Part(8), 1));
                Recipe.Add(new Pair<Part, int>(new Part(9), 1));
                break;
        }
        //===========DB구현시 삭제 요망========== 
    }

    public DroneModel()
	{
		this.ID = -1;
	}

	public int getID() {
		return ID;
	}
	public void setID(int id) {
		this.ID = id;
	}
	public string getTitle() {
		return Title;
	}
	public void setTitle(string title) {
		this.Title = title;
	}
	public int getPrice() {
		return Price;
	}
	public void setPrice(int price) { 
		this.Price = price;
	}
}
public class Pair<T, U>
{
    public Pair()
    {

    }

    public Pair(T first, U second)
    {
        this.First = first;
        this.Second = second;
    }

    public T First { get; set; }
    public U Second { get; set; }
};