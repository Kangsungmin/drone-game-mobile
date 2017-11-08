using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part{
    private int ID;
    private string Title;
    private int BuyPrice;
    private int SellPrice;

    public Part(int id, string title, int buyPrice, int sellPrice)
    {
        this.ID = id;
        this.Title = title;
        this.BuyPrice = buyPrice;
        this.SellPrice = sellPrice;
    }
    public Part(int id)//id를 통해 생성자 생성
    {
        this.ID = id;
        switch (id)
        {
            case 1:
                this.Title = "Box";
                this.BuyPrice = 5;
                this.SellPrice = 4;
                break;
            case 2:
                this.Title = "Plastic";
                this.BuyPrice = 30;
                this.SellPrice = 22;
                break;
            case 3:
                this.Title = "Iron";
                this.BuyPrice = 90;
                this.SellPrice = 72;
                break;
            case 4:
                this.Title = "Aluminum";
                this.BuyPrice = 300;
                this.SellPrice = 240;
                break;
            case 5:
                this.Title = "Silver";
                this.BuyPrice = 2000;
                this.SellPrice = 1600;
                break;
        }
    }
    public Part()
    {
        this.ID = -1;
    }
    public int getID()
    {
        return ID;
    }
    public void setID(int id)
    {
        this.ID = id;
    }
    public string getTitle()
    {
        return Title;
    }
    public void setTitle(string title)
    {
        this.Title = title;
    }
    public int getBuyPrice()
    {
        return BuyPrice;
    }
    public void setBuyPrice(int price)
    {
        this.BuyPrice = price;
    }
    public int getSellPrice()
    {
        return SellPrice;
    }
    public void setSellPrice(int price)
    {
        this.SellPrice = price;
    }
}
