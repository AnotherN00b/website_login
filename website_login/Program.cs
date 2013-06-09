using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace website_login
{
    class Program
    {
        /*
        static bool waitForElement(IWebDriver pDriver, By pBy, string pTxtToLook, int pTimeOut = 5)
        {
            bool LFound = false;
            int LCV = 0;

            while (!(LFound))
            {
                if (LCV > pTimeOut)
                {
                    return (LFound);
                }
                if(pDriver.FindElement(By.<pBy>(pTxtToLook))
                {

                }
            }
        }
        */

        static bool LoadProfile_Link(IWebDriver pDriver, int pTimeOut = 5)
        {
            bool ReturnCode = true;

            //make sure Link exists
            try
            {
                pDriver.FindElement(By.LinkText("Profile")).Click();
                
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: Could not click on 'Profile' link: " + e.Message);
                ReturnCode = false;
            }

            return (ReturnCode);
        }
        static bool validateBasePage(IWebDriver pDriver, int pTimeOut = 5)
        {
            bool returnCode = true;
            int LCV = 0;

            while ((pDriver.Url.ToString().ToLower() != "https://www.linkedin.com/") && (pDriver.Url.ToString().ToLower() != "https://www.linkedin.com"))
            {
                Console.WriteLine("URL is " + pDriver.Url.ToString().ToLower());
                if (LCV > pTimeOut)
                {
                    Console.WriteLine("Timeout waiting for URL!");
                    return (false);
                }
                System.Threading.Thread.Sleep(1000);
                LCV += 1;
            }
            try
            {
                Assert.AreEqual("https://www.linkedin.com/", pDriver.Url.ToString().ToLower());
            }
            catch
            {
                Console.WriteLine("\tURL does not match https://www.linkedin.com/ : " + pDriver.Url.ToString());
                return (false); //If URL does not match, no point in checking anything else
            }

            try
            {
                Assert.AreEqual("World's Largest Professional Network | LinkedIn", pDriver.Title.ToString());
            }
            catch (Exception)
            {
                Console.WriteLine("\tTitle does not match 'World's Largest Professional Network | LinkedIn");
                returnCode = false;
            }

            try
            {
                Assert.AreEqual("Be great at what you do.", pDriver.FindElement(By.CssSelector("span.headline")).Text);
                Console.WriteLine("\tFound: \'Be great at what you do.\'");
            }
            catch (OpenQA.Selenium.NoSuchElementException e)
            {
                Console.WriteLine("Could not find element span.headline" + e.Message);
                returnCode = false;
            }

            try
            {
                Assert.AreEqual("", pDriver.FindElement(By.Id("btn-submit")).Text);
                Console.WriteLine("\tFound: Submit button");
            }
            catch (Exception)
            {
                Console.WriteLine("Could not find submit button");
                returnCode = false;
            }

            try
            {
                Assert.AreEqual("Email", pDriver.FindElement(By.CssSelector("label")).Text);
                Console.WriteLine("\tFound: Email label");
            }
            catch (Exception)
            {
                Console.WriteLine("Could not find label");
                returnCode = false;
            }

            return (returnCode);
        }

        static bool validatePostLoginPage(IWebDriver pDriver, int pTimeOut = 5)
        {
            bool returnCode = true;
            int LCV = 0;


            while (!(pDriver.Url.ToString().Contains("www.linkedin.com/nhome")))
            {
                if(LCV > pTimeOut)
                {
                    Console.WriteLine("\tTimeout occurred waiting for URL to change.  Url is: " + pDriver.Url);

                    return(false);
                }
                System.Threading.Thread.Sleep(100);
                LCV +=1;
                System.Console.WriteLine("Count is " + LCV);
                
            }
            
            try
            {
                Assert.AreEqual("Welcome! | LinkedIn", pDriver.Title);
                Console.WriteLine("\tFound: 'Welcome! | LinkedIn' title");
            }
            catch (Exception)
            {
                Console.WriteLine("\tTitles do not match: " + pDriver.Title);
                returnCode = false;
            }

            
            try
            {
                Assert.AreEqual("people you may know", pDriver.FindElement(By.LinkText("PEOPLE YOU MAY KNOW")).Text.ToString().ToLower()); //need to write a generic wait for element function
                Console.WriteLine("\tFound: 'People you may know' link");
            }
            catch (Exception)
            {
                Console.WriteLine("\tCould not find People You May Know link!");
                returnCode = false;
            }

            /*
            try
            {
                Assert.AreEqual("Account Type:", pDriver.FindElement(By.CssSelector("span > span")).Text);
            }
            catch (Exception)
            {
                Console.WriteLine("\tCould not find Account Type!");
                returnCode = false;
            }
            */
            try
            {
                Assert.AreEqual("LinkedIn Today recommends this news for you", pDriver.FindElement(By.CssSelector("h2.todal-carousel-header")).Text);
                Console.WriteLine("\tFound: 'LinkedIn Today recommends this news for you' link");
            }
            catch (Exception)
            {
                Console.WriteLine("\tCould not find 'LinkedIn Today recommends this news for you' link");
                returnCode = false;
            }

            return (returnCode);
        }

        static bool logIn(IWebDriver pDriver, string pUsrName, string pPasswd)
        {
            pDriver.FindElement(By.Id("session_key-login")).Clear();
            Console.WriteLine("\tSending username");
            pDriver.FindElement(By.Id("session_key-login")).SendKeys(pUsrName);
            pDriver.FindElement(By.Id("session_password-login")).Clear();
            Console.WriteLine("\tSending passwd");
            pDriver.FindElement(By.Id("session_password-login")).SendKeys(pPasswd);
            Console.Write("\tWaiting for Login button to be enabled...");
            for (int LCV = 0; LCV <= 5; LCV++)
                while ((pDriver.FindElement(By.Id("signin")).Enabled) == false)
                {
                    if (pDriver.FindElement(By.Id("signin")).Enabled == false)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
            try
            {
                pDriver.FindElement(By.Id("signin")).Click();
                Console.WriteLine("done!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Caught exception: " + e.Message);
                return (false);
            }
            return (true);
        }

        public static string logFile = Environment.CurrentDirectory + "\\selenium_test.log";
        public static string url = "https://www.linkedin.com";
        static int Main(string[] args)
        {
            string usrName = "";
            string usrPasswd = "";
            IWebDriver driver = new FirefoxDriver();

            //simple usage
            if(args.Length < 1)
            {
                Console.WriteLine("usage: <username> <passwd>");
                driver.Dispose();
                return(0);
            }
            else if ((args.Length > 0) && (args[0].ToString().ToLower() == "help"))
            {
                Console.WriteLine("usage: <username> <passwd>");
                driver.Dispose();
                return (0);
            }
            else if (args.Length == 2)
            {
                usrName = args[0];
                usrPasswd = args[1];
            }
            else
            {
                Console.WriteLine("usage: <username> <passwd>");
                driver.Dispose();
                return (0);
            }

            Console.WriteLine("Initializing...");
            driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 30)); //Make the findelements try finding for up to 30 seconds before failing--helps to resolve some timing issues
            driver.Navigate().GoToUrl(url);
            
            //checks
            Console.WriteLine("Validating site...");
            if(validateBasePage(driver))
            {
                Console.WriteLine("\t->Pass");
            }
            else
            {
                Console.WriteLine("\t->Fail");
            }
            
            Console.WriteLine("Logging in...");
            try
            {
                if (logIn(driver, usrName, usrPasswd))
                {
                    Console.WriteLine("\t->Pass");
                }
                else
                {
                    Console.WriteLine("\t->Fail");
                    return (0);
                }
            }
            catch
            {

            }
            System.Threading.Thread.Sleep(500);
            
            Console.WriteLine("Validating Page after login");
            
            try
            {
                if (validatePostLoginPage(driver))
                {
                    Console.WriteLine("\t->Pass");
                }
                else
                {
                    Console.WriteLine("\t->Fail");
                    return (0);
                }
                
            }
            catch (Exception)
            {
                driver.Dispose();
                return (0);
            }

            Console.WriteLine("Navigating to Profile link");
            try
            {
                if (LoadProfile_Link(driver))
                {
                    Console.WriteLine("\t->Pass");
                }
                else
                {
                    Console.WriteLine("\t->Fail");
                    return (0);
                }
            }
            catch
            {
                Console.WriteLine("\t->Fail");
            }

            driver.Dispose();
            return (0);
        }
    }
}

/*
driver.Navigate().GoToUrl(baseURL + "/");



try
{
    Assert.AreEqual("Matt Cheung", driver.FindElement(By.CssSelector("ul.menu > li.username-cont > a.tab-name.username")).Text);
}
catch (AssertionException e)
{
    verificationErrors.Append(e.Message);
}

driver.FindElement(By.Id("postText-postModuleForm")).Click();
try
{
    Assert.IsTrue(IsElementPresent(By.Id("postText-postModuleForm")));
}
catch (AssertionException e)
{
    verificationErrors.Append(e.Message);
}
try
{
    Assert.IsTrue(IsElementPresent(By.CssSelector("#nav-primary-home > a.tab-name > span")));
}
catch (AssertionException e)
{
    verificationErrors.Append(e.Message);
}
try
{
    Assert.IsTrue(IsElementPresent(By.CssSelector("#nav-primary-profile > a.tab-name > span")));
}
catch (AssertionException e)
{
    verificationErrors.Append(e.Message);
}
try
{
    Assert.IsTrue(IsElementPresent(By.CssSelector("#nav-primary-contacts > a.tab-name > span")));
}
catch (AssertionException e)
{
    verificationErrors.Append(e.Message);
}
try
{
    Assert.IsTrue(IsElementPresent(By.CssSelector("#nav-primary-groups > a.tab-name > span")));
}
catch (AssertionException e)
{
    verificationErrors.Append(e.Message);
}
try
{
    Assert.IsTrue(IsElementPresent(By.CssSelector("#nav-primary-jobs > a.tab-name > span")));
}
catch (AssertionException e)
{
    verificationErrors.Append(e.Message);
}
try
{
    Assert.IsTrue(IsElementPresent(By.LinkText("Home")));
}
catch (AssertionException e)
{
    verificationErrors.Append(e.Message);
}
try
{
    Assert.AreEqual("Profile", driver.FindElement(By.CssSelector("#nav-primary-profile > a.tab-name > span")).Text);
}
catch (AssertionException e)
{
    verificationErrors.Append(e.Message);
}
try
{
    Assert.AreEqual("Contacts", driver.FindElement(By.CssSelector("#nav-primary-contacts > a.tab-name > span")).Text);
}
catch (AssertionException e)
{
    verificationErrors.Append(e.Message);
}
try
{
    Assert.IsTrue(IsElementPresent(By.LinkText("Groups")));
}
catch (AssertionException e)
{
    verificationErrors.Append(e.Message);
}
try
{
    Assert.AreEqual("Jobs", driver.FindElement(By.CssSelector("#nav-primary-jobs > a.tab-name > span")).Text);
}
catch (AssertionException e)
{
    verificationErrors.Append(e.Message);
}
try
{
    Assert.AreEqual("Inbox", driver.FindElement(By.CssSelector("#nav-primary-inbox > a.tab-name > span")).Text);
}
catch (AssertionException e)
{
    verificationErrors.Append(e.Message);
}
try
{
    Assert.AreEqual("Companies", driver.FindElement(By.CssSelector("#nav-primary-company > a.tab-name > span")).Text);
}
catch (AssertionException e)
{
    verificationErrors.Append(e.Message);
}
try
{
    Assert.AreEqual("News", driver.FindElement(By.CssSelector("#nav-primary-news > a.tab-name > span")).Text);
}
catch (AssertionException e)
{
    verificationErrors.Append(e.Message);
}
try
{
    Assert.AreEqual("More", driver.FindElement(By.CssSelector("#nav-primary-more > a.tab-name > span")).Text);
}
catch (AssertionException e)
{
    verificationErrors.Append(e.Message);
}
driver.FindElement(By.LinkText("Inbox")).Click();
try
{
    Assert.IsTrue(IsElementPresent(By.CssSelector("em")));
}
catch (AssertionException e)
{
    verificationErrors.Append(e.Message);
}
try
{
    Assert.IsTrue(IsElementPresent(By.CssSelector("li.nav-inbox.active > a")));
}
catch (AssertionException e)
{
    verificationErrors.Append(e.Message);
}
try
{
    Assert.IsTrue(IsElementPresent(By.LinkText("Sent")));
}
catch (AssertionException e)
{
    verificationErrors.Append(e.Message);
}
try
{
    Assert.IsTrue(IsElementPresent(By.LinkText("Archived")));
}
catch (AssertionException e)
{
    verificationErrors.Append(e.Message);
}
try
{
    Assert.IsTrue(IsElementPresent(By.LinkText("Trash")));
}
catch (AssertionException e)
{
    verificationErrors.Append(e.Message);
}
driver.FindElement(By.CssSelector("ul.menu > li.username-cont > a.tab-name.username")).Click();
driver.FindElement(By.LinkText("Sign Out")).Click();
*/