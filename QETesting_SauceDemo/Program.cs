using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;

namespace QETesting_SauceDemo
{
    [TestFixture]
    
    class Program
    {
        //Create a reference for Chrome Browser
        IWebDriver driver = new ChromeDriver();

        static void Main(string[] args)
        {

        }

        [SetUp]
        
        public void Initialize()
        {
            // Go to saucedemo page
            try
            {
                driver.Navigate().GoToUrl(Constants.TestUrl);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(200);
            }
            catch (Exception ex)
            {
                Console.WriteLine(Constants.WebpageAccessError,ex);
            }

        }


        [Test]
        public void PurchaseOneItemE2ETest()
        {
            userLoginPageCheck();

            //Navigate to inventory page
            //Get element with class name 'inventory_list'
            IWebElement element = driver.FindElement(By.ClassName("inventory_list"));

            //Iterate the list and find the inventory item called sauce labs fleece jacket
            //Add to cart and breakout of loop
            IList<IWebElement> elements = element.FindElements(By.ClassName("inventory_item"));
            foreach (IWebElement e in elements)
            {
                var addToCartButton = e.FindElement(By.TagName(Constants.button));
                var addToCartButtonID = addToCartButton.GetAttribute(Constants.id);

                if (addToCartButtonID.Equals("add-to-cart-sauce-labs-fleece-jacket"))
                {
                    addToCartButton.Click();
                    break;
                }

            }

            //Find and check Shopping cart is not empty

            IWebElement shoppingCartButton = driver.FindElement((By.Id("shopping_cart_container")));
            IWebElement shoppingCartBadge =shoppingCartButton.FindElement(By.XPath("//*[@id=\"shopping_cart_container\"]/a/span"));
            Assert.IsNotNull(shoppingCartBadge);

            //Navigate to shopping cart page
            shoppingCartButton.FindElement(By.TagName("a")).Click();

            shoppingCartPageCheck("1");

            deliveryPageCheck();

            orderSummaryPageCheck();

            completeCheckoutPageCheck();
        }

        //Test login page
        public void userLoginPageCheck() {

            //Find Username element
            IWebElement userName = driver.FindElement(By.Id(Constants.userNameElement));

            //Enter username 
            userName.SendKeys(Constants.standardUserName);


            //Find password element
            IWebElement userPassword = driver.FindElement(By.Id(Constants.passwordElement));

            //Enter password 
            userPassword.SendKeys(Constants.standardPassword);

            //Find login button
            IWebElement userLogin = driver.FindElement(By.Id(Constants.loginButton));

            //Click on login button
            userLogin.Click();

        }

        public void shoppingCartPageCheck(String numberOfItems) {

            //Check shopping cart page displays 1 item
            IWebElement cartItem = driver.FindElement(By.ClassName("cart_item"));
            Assert.IsNotNull(cartItem);
            var cartQty = driver.FindElement(By.ClassName("cart_quantity")).GetAttribute(Constants.textContent);
            Assert.AreEqual(numberOfItems, cartQty);

            //click on checkout button
            IWebElement checkoutButton = driver.FindElement(By.Id("checkout"));
            checkoutButton.Click();

        }


        public void deliveryPageCheck() {
            //Enter customer first name
            IWebElement firstName = driver.FindElement(By.Id("first-name"));
            firstName.SendKeys("MyFirstName");

            //Enter customer last name
            IWebElement lastName = driver.FindElement(By.Id("last-name"));
            lastName.SendKeys("MyLastName");

            //Enter customer postcode
            IWebElement postCode = driver.FindElement(By.Id("postal-code"));
            postCode.SendKeys("3000");

            //Click on Continue button
            IWebElement continueButton = driver.FindElement(By.Id("continue"));
            continueButton.Click();

        }

        public void orderSummaryPageCheck() {
            //Click on Finish button
            IWebElement finishButton = driver.FindElement(By.Id("finish"));
            finishButton.Click();
        }


        public void completeCheckoutPageCheck() {

            //Check elements on the order complete page
            IWebElement orderComplete = driver.FindElement(By.ClassName("checkout_complete_container"));
            Assert.True(orderComplete.FindElement(By.TagName(Constants.img)).Displayed);

            String orderCompleteHeader = "Thank you for your order!";
            var orderSuccessHeader = driver.FindElement(By.ClassName("complete-header")).GetAttribute(Constants.textContent);
            Assert.AreEqual(orderSuccessHeader, orderCompleteHeader);

            String orderCompleteMessage = "Your order has been dispatched";
            var orderSuccessMessage = driver.FindElement(By.ClassName("complete-text")).GetAttribute(Constants.textContent);
            Assert.True(orderSuccessMessage.Contains(orderCompleteMessage)); ;

            //Click on Back to home button
            IWebElement backToHome = driver.FindElement(By.Id("back-to-products"));
            backToHome.Click();
        }



        [TearDown]
        public void CloseTest()
        {

            //Close the browser
            driver.Quit();

        }


    }
}
