using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;

namespace FirstTests.Pages;

public class InventoryPage : BasePage
{
    private readonly By tieuDeTrang = By.CssSelector(".title");
    private readonly By oSapXep     = By.CssSelector("[data-test='product-sort-container']");
    private readonly By giaSanPham  = By.CssSelector(".inventory_item_price");

    public InventoryPage(IWebDriver driver) : base(driver) { }

    public bool DaHienThi() => Tim(tieuDeTrang).Displayed;

    public InventoryPage SapXep(string value)
    {
        new SelectElement(Tim(oSapXep)).SelectByValue(value);
        return this;
    }

    public List<double> LayDanhSachGia()
    {
        Tim(giaSanPham); // chờ danh sách xuất hiện
        return driver.FindElements(giaSanPham)
                     .Select(e => double.Parse(e.Text.Replace("$", "")))
                     .ToList();
    }
}
