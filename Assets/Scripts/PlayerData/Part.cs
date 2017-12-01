using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part{
    private int ID;
    private string Title;
    private int BuyPrice;
    private int SellPrice;
    public int[] Recipe = new int[8] {0,0,0,0,0,0,0,0};

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
            case 6://Plastic(100)
                this.Title = "Propeller";
                this.BuyPrice = -1;
                this.SellPrice = -1;
                Recipe[2] = 100;
                break;
            case 7://Al(10)+Iron(50)+Plastic(30)
                this.Title = "Motor";
                this.BuyPrice = -1;
                this.SellPrice = -1;
                Recipe[4] = 10;
                Recipe[3] = 50;
                Recipe[2] = 30;
                break;
            case 8://Plastic(250)+Al(20)+Silver(1)+Paper(30)
                this.Title = "Frame";
                this.BuyPrice = -1;
                this.SellPrice = -1;
                Recipe[5] = 1;
                Recipe[4] = 20;
                Recipe[2] = 250;
                Recipe[1] = 30;
                break;
            case 9://Al(30) + Iron(100) + Plastic(20) + Paper(50)
                this.Title = "Mainboard";
                this.BuyPrice = -1;
                this.SellPrice = -1;
                Recipe[4] = 30;
                Recipe[3] = 100;
                Recipe[2] = 20;
                Recipe[1] = 50;
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
