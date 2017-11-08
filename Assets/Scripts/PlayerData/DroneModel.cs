using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneModel {

	private int ID;
	private string Title;
	private int Price;
    
    public DroneModel (int id, string title, int price)
	{
		this.ID = id;
		this.Title = title;
		this.Price = price;
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
