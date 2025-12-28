'BTPricingServices
'
'Description - New Pricing Web Service for initial use by NextGen which cannot quite
'   handle Tolas Pricing on the UI. The calling application will specify SystemID SystemPassword,
'   AccountID and ItemIDs along with some other parameters in a repeating collection of items.  
'   The Method/Service will decide whether to break these requests up into mulitple calls to the
'   Tolas listener, which can only handle a max of 900.  Note that this is ONLY for Tolas.
'   
'   Since there will be a mulitple line/item architecture, this will only allow a Class
'   for input.  In the interest of time, this will be a .NET 3.5 Web Service, as I instantly
'   had issues and errors just getting a WCF to work.  May be minor, but no time to climb the
'   learning curve.
'
'Developer - Marvin Perkins
'
'Work Started: 05-23-2011
'
'First Testing to BTE: 05-26-2011
'
'First Testing from NextGen Application: Unknown, but made available as of 05-27-2011
'
'Production Ready: 06-10-2011
'
'Production Date:
'
'Notes:
'   The "double commented" notes below were just moved from BTStockCheckServices for reference.  The "real" notes
'   begin as of 05-23-2011 below.
'
'' ''01-06-2011
'' ''   Decided to start with Web Service in VS.2008.  Might be recoded as WCFService, as it is probably small enough to
'' ''   do so without much effort, but went with the comfort zone for the initial effort.  This small service app 
'' ''   may also be a good place to begin exercising C# as well.
'' ''
'' ''   For some reason, it seems that when you change the Class name in the code behind to something other than
'' ''   the default "Service1" the physical .asmx file in the project is not updated.  It can be corrected by
'' ''   manually editing the file outside VS.  This does not make any sense, but it was an initial issue for me.
'' ''
'' ''   Note that including the "<WebMethod> _ " on the line prior to the Function allows it to show up in your
'' ''   presentation ASMX page.  It has been long enough since I have written a Web Service that I'm going through
'' ''   a refresher!
'' ''
'' ''01-07-2011
'' ''   Started cloning appropriate areas of the old StockCheckWS Web Service to get some basic functionality.
'' ''   Since the construction/development of this is not even due to start yet, I stopped work around noon
'' ''   so that I could concentrate on Design-related tasks assigned to me and familiarizing myself with the
'' ''   Design Document Templates/Guidelines Ivor wants us to use, as well as the Design Tasks that are co-assigned
'' ''   to Balaji and me and/or Ivor and me.  Have not started configuring the IP calls.
'' ''
'' ''01-18-2011
'' ''   OK back again, since I am fairly restricted on what design work I can do.  I believe we are now to 
'' ''   the point of looking at the Tolas/SOP spec to begin formatting the request and preparing to parse
'' ''   the response.  Meanwhile, in working with BizTalk to figure out how to talk to a Web Service, I 
'' ''   finally realized how to use a class as an input, so once fully functional, I will see what it would
'' ''   take to code for parameters as well as a class.  My thought is the two web methods call a single Sub/
'' ''   Function, with the parameter driven call simply converting to a class before calling the class driven
'' ''   sub.  In fact, I am going to restructure now to enable that approach.
'' ''
'' ''   Done.
'' ''
'' ''01-25-2011
'' ''   Will be taking the model from my .NET TCPClient App which takes a string from a file and sends it blindly to
'' ''   a listener taking whatever comes back to redo the call from the WebService here.  Also, will do a more
'' ''   modular approach in order to segregate out the building of the request, making the ListenerCall, and 
'' ''   parsing of the response to different Sub/Functions.  I will also adjust my object/class instantiating
'' ''   to be flexible enough to generate or not generate empty XML based on what the calling NextGen developers
'' ''   prefer.
'' ''
'' ''   In addition, I want to adopt the AppData.xml approach to all my table lookups.  Still thinking this will
'' ''   be small enough to redo as WCF and also as C#.
'' ''
'' ''01-26-2011
'' ''   Created the AppData xsd and xml files, cloned and adjusted the LoadAppData Sub, and placed it in the "New"
'' ''   event of the StockCheck class, which, I believe, allows it to get loaded with the page prior to
'' ''   performing any web Method Subs.  Enabled EmergeMail, although I'm not sure of the ramifications being
'' ''   a web service being called and created potentially so many times per minute/hour/day.
'' ''
'' ''   Also enabled simple logging using an adapted model of original StockCheckWS, with adjustments.  With both
'' ''   email and logging, there is a webconfig flag for disabling for performance and/or nuisance reasons.
'' ''
'' ''01-28-2011
'' ''   After converring with GSN for advice, will now use the List object instead of ArrayList (which can't be
'' ''   assigned a Type up front) or Array (which needs to be ranged).  List can be added to dynamically via .Add
'' ''   just like an Arraylist, but you also assign it an "OF" type so that it seamlessly integrates into the
'' ''   Soap constructed XML response class.  I also got a quick course on using <XmlElement>, <XmlArray>, 
'' ''   <XmlArrayItem>, and <XmlAttribute> as noted in my Class definitions below.  This was a huge help and
'' ''   time saver for me.
'' ''
'' ''01-31-2011
'' ''   Completed the existing AppData lookups, removed unused class members, defined <xml> values for Class
'' ''   members.  Analyzed and queried BTE on response anomalies.  Queried BTB on blanket 156 byte responses
'' ''   (with blank WHS segments).  Refined ResponseStatusMessage construction.  Added some abort logic and
'' ''   functionality.  Descided to enter AccountIDTypes and ItemIDTypes into AppData, even if NextGen doesn't
'' ''   need or care.  Note that Replacement logic is there as well, even though no mention of it in UseCases.
'' ''
'' ''   Also, note that we can use the <Xml...> attributes to make the variable names conform to the NG dev
'' ''   standards if needed.  And thus ends the snowiest January on record in the NYC metro area.
'' ''
'' ''03-08-2011
'' ''   Back to round out the code so it can be posted for access on edi.btol.com.  It will point to ISTEST
'' ''   until the ERP listeners are migrated to USER.  Riz is still doing some slop.  Tried and ISBN (not
'' ''   found) and he gave me a bad length and didn't give me the inventory segment (ItemID) back.
'' ''
'' ''03-09-2011
'' ''   With GSN suggestion, Published to BWTWSDEV02, one of the two replacement machines for the WebServices
'' ''   boxes.  Had to use File System publish, as it doesn't appear that FrontPage Server Extensions are installed.
'' ''   Interesting note that if you run "As Administrator" the drive mappings/permissions may not work!  You get
'' ''   a "Publish Failed", but no additional data.
'' ''
'' ''   Note that even though .NET framework 3.5, the ASP.NET version for the application pool should be 2.0.
'' ''   I got web config errors (duplicate entry errors) when run under v 4.0.  I may try this again to verify.
'' ''
'' ''   Moved BTStockCheckServicesAppData.xml to the App_Data folder.  Note that the xmldata appears to be
'' ''   moved into memory upon "invoke" rather than when the .asmx page loads.  We may want to explore some other
'' ''   mechanism for getting the stuff into memory somehow.
'' ''
'05-23-2011
'   After upgrading BTStockCheckServices to VS.2010, realized that targeting .NET 3.5 was all that was needed
'   in VS.2010 to have the Web Service project option show up.  So, although targeting .NET 3.5 and a Web Service
'   instead of WCF, we are at least in VS2010.
'
'   Again had to manually change the asmx file to reflect changing the service name from Service1 to BTPricingService.
'
'   Constructed all the supporting code before actually starting the code that will do the work. Kept most of the
'   validity checks and translations in place, though many might remain unused or unneeded.  Most of the AppData.XML
'   simply remained as is, except changed the string for a successful transaction to "Successful Query".
'
'   Note that since a status will be returned for every line item, we will be passing it on every line item, and
'   we will need to reflect an overall status in the header by keeping track somehow of a tally and/or type of
'   detail errors encountered.
'
'   Everything now should be in place to begin the meat of the coding to take the input class, construct
'   the Tolas RequestString(s), make the Tolas Listener Call(s), and translate and construct the Response Class.
'
'   Not bad for a day of coding.
'
'05-26-2011
'   Finalized the Request-Response Classes (XML) and published the WebService to EDIDEV.btol.com.  Did not distribute
'   because I want to build as much as the handling code as possible first to make suer my "Finalized" is, in fact,
'   final.
'
'05-27-2011
'   Got the Single (Paramenter Entry) request to work, without parsing the response.  Also, the marshalling logic
'   for making mulitple calls to Tolas is not yet in place.
'
'06-03-2011
'   Alot of stuff has happened but not been noted due to time constraints.  Employed the use of SoapUI to test multiple
'   sends to Tolas.  This uncovered code bugs, so continued to use it until the requests for multiple lines (flat text)
'   looked good.  Once that was done, created flat file (single line files) requests for 10, 100 and 300 bytes in order
'   to test using the Generic Client Windows App.  
'
'   Tested for several days with Tolas to get them to get the entire request, finally relying on an EndOfTransmission
'   character chr(3) or Hex03, which was fairly easy to append.  Then it was my turn.  Although the Windows Client App
'   was receiving everything flawlessly could not understand why the Web Service App, using the same TCPIP.client, kept
'   stopping before the data was finished coming in.  Banging my head for hours on this, finally gave up on figuring out
'   why, when adding the "wait" for data to become available made everything work just fine.
'
'   Finally decided (and found a supporting case on the Internet) to ask Tolas to send the same EOT marker that I would look
'   for as well, and this works like a charm.  I left in the ability to do a brute force wait as well, incase there may not
'   be a chance to get an EOT marker in some future instance.  At least the framework is here.
'
'   There is a lot of debugging file writing currently, but it can all be turned of in the web.config.
'
'   Cleaned up the code, and now we are on to the "Marshalling" piece.
'
'   Well, ALMOST there.  I sucessfully break out during the line processing and call Tolas as many times as needed,
'   getting the response strings in succession, but the ResponseObject is not properly constructed when we take the
'   multi-call route.  It's odd becuase the same code is called in both places (cloned) except for the flag that says to 
'   appendonly or not.  But even that is the same the first time around, and it doesn't seem to construct right even if
'   we break out after the first set of reads...
'
'   This seems like something simple that is just being overlooked, but hard to debug as a Web Service.  I may need to
'   Manipulate the flat string response to only append the lines to a pre-formed header, and then parse it out....
'   ...But I'll look for something more simple first by making small requests with low thresholds to further analyze
'   my pieces.... but its almost 6pm on a Friday.  I'm done for now.  The service in place works for up to 300 lines.
'
'06-04-2011
'   It looks like the marshaling is working now.  There were a few issues.  I was not stripping of the Hex03 EOT marker
'   when I did a non-append read.  this was not an issue if there was only 1 read, but as soon as I slapped another set
'   of text lines on it, it screwed up the Parsing and XML structure.
'
'   I also was not passing the proper line count to Tolas, but rather passing the total linecount.  This caused tolas to
'   try and read beyond my last line, interpreting the Hex03 as an invalid inventory type.
'
'   So, 
'       1)I changed the overall approach to building/accumulating the entire string before parsing into the response object.
'       2)I always Trim off the trailing Hex03 after a read.
'       3)I re-create the TolasRequest Header before each call with the proper line tally count.
'       4)I pass the full request line count to the Parse routine so it will override the initial Tolas Header count with the total.
'
'   I don't know how much overhead I have introduced with all the string functions and logging, etc. but tested a 900 unit request
'   and got a turnaround of 15 seconds.  At least it is working correctly!  Next is code cleanup and do some unlogged tests to see
'   if we can get a better turnaround... followed by a 9000 test... ulp.
'
'06-08-2011
'   Goodbye Molly, we shall miss you severely.
'
'   Tolas down most of the day, and the BTPicing Listener doesn't appear to be up yet.
'   That showed/reminded me that I am not handling Errors well in the Response Object yet, so I altered the code to roughly do so.
'   Was going to re-test some response times here at work, but no-can-do.
'
'   Meanwhile, VietNam seems to be testing single line lookups, mostly with invalid accounts.  I did see some unexpected results, which
'   could be a factor of my Error Handling above, but I also added back in the Receive and Send Strings since they are doing 1-liners,
'   to see what is actually going up and coming back.
'
'   I'll take that back out when I get to the bigger requests again.
'
'06-10-2011
'   Added the code for creating a transaction number by importing GetHexName and Padit from MEPCALLS.  GetHexName(12) is the base number
'   and _x is added to it where x is the number of calls to Tolas.  Two notes are that due to mulitple calls that could be simultaneous,
'   the GetHexName(12) won't necessarily be unique, and since we dynamically split and call Tolas, we don't try to figure out ahead of
'   time how many calls we will be making, as in adding _1of3 instead of just _1... but maybe if it's not too painless, I will try it.
'
'   Also, fixed a problem with the length of the RecvStr when appending, and trapped (with no action) for any exceptions while writing to the 
'   log files.
'
'   1of3 type logic for the multiple calls was not painful by using int and mod.  With a max of 34 calls (9,999 line order), there is just 
'   enough room in the header for the 12-byte hexname followed by "_34of34" with a byte to spare.
'
'   Now checking for a request that contains over 9999 line items and aborting if true.
'
'   Today is the day to mock up the 10K line request and try it....
'
'   After setting the maxRequestSize in the config, and extending the timeouts, it all worked fantasticly!  Here or the BWT benchmarks,
'   inclusive of the full round trip from calling/receiving application (SoapUI):
'       40 line = .5 sec
'       900 line = 6 sec
'       9900 line = 68 sec.
'
'07-13-2011
'   Looked at blocking blank itemids (or obvious invalid ones) but re-realized that I can't do that with multiline requests.  It would be
'   quite cumbersome to try and make an exception for single line requests, but put it in, even though we break out of a ForEach loop and
'   do an abrupt Return.  I think that is ok, since we are a single shot app.
'
'07-28-2011
'   Started looking at the different web.config build options in order to avoid my overwrite problems when publishing, and to make use
'   of the VS.2010 built in transform options based on the build.  Ran into a snag in trying to determine where you signify what build 
'   you want to run, and discovered that Configuration Manager as well as the build options on the Web and Compile tabs of the project
'   properties were missing.  A web search revealed that these were dictated by Tools-Options-Projects and Solutions-General and a check box
'   for "Show Advanced Build Configurations."  I also checked "always show solution".  Of course, this took a few hours of frustration,
'   but I feel a lot better for knowing this.  I may have found a way to avoid those "invalid solution files", but not sure yet.
'
'   Now I can actually go on to the different web.configs, and see if I can make it work.  This is all with the objective of publishing
'   my web services to edi.btol.com.
'
'   OK, same day, several more hours later.  Banging my head against the wall not realizing that "Publish" is different than "Build"!
'   I have been "Build"-ing for hours wondering why I couldn't make these Web.config Transforms to work... or why I could never even see
'   the results of my "Package" since 7-13-2011.  Duh, you need to "Publish" it to do the Web.config Transforms and create a Package.
'   "Build"ing it does nothing.  Publish will use the settings and transforms that are specified in the Configuration Manager, 
'   ...and then you select the Publishing Profile to choose the output destination!
'
'   I'm just glad I got this finally figured out (except the finer points which I will do tomorrow) before leaving today.  So, nothing
'   like spending all day just trying to figure out how to get this new feature (Web.Config Transforms) to work.  By the way, after
'   creating the different build types with the configuration manager, right click on the web.config file to create the transform
'   files for them.  I THINK I will be able to get this work now.
'
'07-29-2011
'   Yes.  Note that the PublishingProfile determines the destination for the project/application files and the 
'   Build Configuration (Manager) determines the settings (inclusive of Web.config transformations) to use.
'   So, we have one Publishing Profile for DEV & QA, but the Web.config that is constructed (IP address of Tolas Listener)
'   is detemined by the Build configuration of DEV or QA.  The Publishing Profile just uses whatever Build Configuration is active.
'   That way, I can Publish PROD using the DEV Build Configuration, so that the PROD url points to Tolas DEV back end, which
'   is what is intended for PE testing.
'
'   This will also be perfect when I want to disable POST to shut off Parameter Entry facing the outside world.
'
'12-02-2011
'   It's been a while.  Noticed that when an invalid account is used, Tolas does not return any line information, so that the
'   detail string is just the terminator chr(3).  Revised the "get Status Description" method to alter the message to "No Status Message Returned"
'   instead of including the chr(3) in the format "[code]:No Description Found For This Code" which was ugly.
'
'   Also, changed the byParameters method to use Strigs for decimal values so I could default them to zeros if omitted.  Otherwise,
'   you get a .NET error / Page Cannot Be displayed.
'
'05-22-2012
'   Apparently, when doing the Soap Call, the trailing space on BTKey is being maintained (from parsing) in the Soap Object which 
'   caused a problem with the UI.  Added a Trim to the 2 strings in the line-parsing routine.
'
'07-19-2013
'   Questions have come up about PE with regard to this service and listener, even though the base call is only 300ms or so.  To aid in
'   research and troubleshooting, I have added "Time Elapsed" points in the debug log.  The WS itself only accounts for somewhere between
'   50-75ms.  Tolas about 100ms, the rest is latency.  Also, far from what was originally designed (capable of taking up to 9999 lines in a 
'   shot) the UI defaults to 9 items per call, often less than that, and 30 items absolute max.  Whether 1 or 30 lines, the response time
'   is virtually the same.
'
'07-23-2013
'   Will convert LoadAppData in the New Sub to the more efficient single-line read in global.asax.  I don't think it will change the response
'   time much, if at all, but it is architecturally much more sound.  I'll then turn off the detailed logging and try to do some tests to see
'   if any noticable impact is observable.
'
'   Note that this AppData differs from BTStockCheckServices in that this is the older collections without Parents... in other words, there
'   is just a collection of Warehouse records rather than a collection of Warehouse records underneath a Warehouses Parent.  Works fine, though.
'   The Class understands that AppData.Warehouse is a collection of unbounded Warehouse records.  Having the Warehouses parent just makes
'   the syntax nicer because the collection is then called AppData.Warehouses.
'
'   Also new for this global.asax, I removed the instantiations from Sub New() and instead put them as Public Delclarations that also
'   instantiate themselves at the beginning of the Class.  This combines a couple of lines to one, and shows the reference to global.asax
'   right at the beginning of the (asmx) Class.
'
'   In addition, I now am using the AppPath as found in global.asax, and similarly declaring and instantiating it at the beginning of the
'   Class.  Cleaner and eliminates several lines of "Dim" statements.
'
'08-13-2014
'   I have been working the elusive "ELMAH Logs" ticket, where they are getting null instance errors 2 out of three times, with the third
'   apparently always succeeding.
'
'   Over the last couple of days, I have adjusted try-catches, logging, and the global.asax to try and dig down to what is going on.  I now have
'   a clean way to capture the Soap Request, even if it never hits the function call.  I just hit on it finally after 2 days, but I think I can throw
'   anything at it, and as long as it hits the web service and "points" to the method, I'll log the Soap, even if it is mal-formed in content.
'
'   More testing/refining tomorrow.  Then it will be a question of trying to replicate in QA/DEV or just put it in Prod and wait.
'
'08-18-2014
'   Much more work put into this. I am now logging in the regular non-debug log when the HTTP response is not 200, and am also logging when the 
'   Application_Error fires, which does not happen every time an Exception happens in the code, by the way, but seems to be only at the higher
'   Application level.  Several things noted:
'
'       1) I was not catching exceptions from the topmost point in my three primary methods.  I am now.
'       2) Global asax is extremely helpful, and the events can log to normal log files since Application_Start reads AppData...xml.
'       3) Begin/End Request (in Global) fire even if the request does not "hit" the actual method/function.
'       4) As noted above, Application_Error does not fire on every Exception, but only those at the Global/Application level, apparently.
'       5) So far, in QA/DEV, we observed what might be one instance of a Null Reference Exception when a valid Soap Message was passed.
'       6) The above (5) happened right as/after the Application_Start fired following an idle period timeout "sleep".
'       7) This recyle feature is not controlled by the Application Pool Recycle options, but instead by Advanced Settings - Idle Timeout which defaluts to 20 min.
'       8) As of a short time ago, set the idle timeout to 0 (never) and also added logging the stack.trace, with a catch if it cannot.
'
'   Based on all of the above, I am wondering if due to the idle time restarts, every once in a while, the PricingServices function might be starting to
'   instantiate the global.asax objects before global.asax has finished "reading" them?  I don't know if that is even possible, but I am going to go back and review
'   the one time that it looks like we may have had the situation, verify that it does appear to be a Null Reference Exception with a good Soap Message, do any
'   fine tuning of the logging/catching, build and re-publish to DEV/QA, let it run for a time, and then deploy to PROD.
'
'   Set PROD and QA/DEV App Pool settings to match.  Rycycle at midnight, log recycles to the EventLog for all conditions, idle timeout-0.
'
'   Running fine in QA/DEV but problem is difficult, if not impossible, to replicate there.  Deploying to edi.btol.com (Prod).
'
'08-19-2014
'   Deploying to prod set off a flurry of HTTP 500s, and noticed from the debug log that requests are overlapping due to volume and frequency... which 
'   begged the question if logging is contributing to the issue.  Disabled debug logging shortly before 5pm yesterday, and the only Null Reference looks 
'   like it was caught and handled by the function, returning a "Successful Query" (suprisingly enough) even with the Exception logged in the standard log.
'
'   May need to explore setting up logging arrays and then periodic writes as has been done in the past.  Perhaps with a timer and objects initiated globally
'   in global.asax.  Meanwhile, will leave debug logging off, and have CSC test ELMAH logs as is.  If Debug Logging is required to track down remaining issues,
'   will turn it on and also look at Arrays/Timed Writes for all logging at that time.
'
'08-22-2014
'   After reviewing logs and debug logs(QA), discovered that AppPath is not being set until GlobalLogMess is called rather than being set when Application_Start.
'   This is becuase that log write was originally coded directly in Application_Start, so AppPath was set there, but when the code for logging got moved
'   to GlobalLogMess, that setting of AppPath also got moved, so when referenced in Application_Start, it can be a null reference if GlobalLogMess has not been 
'   called previously.
'
'   AppPath is now set in Application_Start.  Also, all the logging subs have also set path to optional, and if it is the default of "", it is set to find the
'   Physical ApplicationPath by the appropriate method.
'
'   Also, added 9 retries on logging (all subs) with 100 msec sleep in between in the event of exceptions.  Required adding System.Threading to Global.asax.vb
'
'   Finally, added StackTrace to Exception Catches that are written to the Debug Log from RequestByParameters, RequestByClass, and GetTolasPricing.
'
'   Deployed to QA/DEV and Prod.
'
'08-25-2014
'   Got the TFS ticket back after EMLAH errors were observed in PROD.  They were all before the above changes were deployed, BUT I did get a HTTP 500 in QA AFTER
'   The changes were deployed, and due to the help of the stacktrace, it pointed to trying to close the logger in the catch  before it had been successfully instantiated.
'   I believe this might happen with the log file is in use.  (again, more reason to have it timer based writing a collection of errors at the Global level)
'
'   Now all (5) of the logging subs check for the existance of logger during the catch before trying to close it.
'
'   Deploying to DEV/QA and PROD (note the dll shows the timestamp of the deploy, not the last code change/build)
'


Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

'These were added by me based on the old StockCheckWS
'   All of them might not be needed.
Imports System.Net
Imports System.Net.Mail
Imports System.Net.Sockets
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading



' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://edi.btol.com/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class BTPricingService
    Inherits System.Web.Services.WebService

    Public RecvStr As String = ""
    Public AppData As BTPricingServices.AppData = BTPricingServices.Global_asax.MyAppData
    Public AppPath As String = BTPricingServices.Global_asax.AppPath
    Public StartTime As Date = Now

    'Classes expected in the Request
    Class PricingRequest
        Public SystemID As String
        Public SystemPassword As String
        Public AccountID As String
        Public WHSCOD As String
        Public ItemList As List(Of ItemRequestDetail)

    End Class

    Class ItemRequestDetail
        Public ItemId As String  'UPC/GTIN for BTE, BTKey for BTB
        Public Quantity As Integer
        Public ReturnFlag As String
        Public PriceKey As String
        Public ProductLine As String
        Public ListPrice As Decimal
        Public AcceptableDiscount As Decimal
    End Class

    'Classes Constructed for the Response
    Class PricingResponse

        'NOTE: go back and comment out, then remove what isn't to be used.
        <XmlElement("StatusMessage")> Public HeaderMessage As String
        <XmlElement("AccountID")> Public AccountID As String
        <XmlElement("WHSCode")> Public WHSCOD As String
        <XmlElement("ItemCount")> Public ItemCount As Integer

        'OK...  <XmlArray(String)> means this class will be an opening tag of this name
        '           (instead of the Class Name) followed by repeating members.
        '       <XmlArrayItem(String)> means each member should fall under one tag of this name
        '            (instead of the "Of" Class Name)
        '       <xmlElement(String)> used here would supress the class grouping tag and just cause the
        '          individual members to be repeating under the name supplied.
        '
        '       Finally, <XmlAttribute(string)> can't be used here, but on an individual child turns it
        '           into an attribute of the parent with the attribute name as specified.


        '<XmlElement("WhoreHouse")> _
        <XmlArray("ItemsList")> _
        <XmlArrayItem("Item")> _
        Public ItemResponseDetails As New List(Of ItemResponseDetail)

    End Class

    Class ItemResponseDetail
        <XmlElement("ItemID")> Public ItemID As String
        <XmlElement("ItemType")> Public ItemType As String
        <XmlElement("DiscountPercent")> Public DiscountPercent As Decimal
        <XmlElement("DiscountPrice")> Public DiscountPrice As Decimal
        <XmlElement("LineStatusMessage")> Public DetailMessage As String
    End Class


    'Classes not used in Request or Response but used by code.
    Class SKUInfo
        Public ItemID As String
        Public Source As String
        Public Valid As Boolean
        Public ItemFlag As String 'I or U for ISBN or UPC based
        Public EANFlag As Boolean 'this is for anything over 12 bytes
    End Class

    Class CusIDInfo
        Public CusNo As String
        Public Type As String
        Public Valid As Boolean
    End Class

    'AppData Specific Classes
    'Class Warehouse
    '    Public Code As String
    '    Public Description As String
    'End Class
    'Public Warehouses As ArrayList

    'Class StatusMessage
    '    Public Code As String
    '    Public Description As String
    'End Class
    'Public StatusMessages As ArrayList

    'Class AccountIDType
    '    Public Code As String
    '    Public Description As String
    'End Class
    'Public AccountIDTypes As ArrayList

    'Class ItemIDType
    '    Public Code As String
    '    Public Description As String
    'End Class
    'Public ItemIDTypes As ArrayList

    'Class EmailRecipient
    '    Public Name As String
    '    Public Address As String
    '    Public Group As String
    'End Class
    'Public EmailRecipients As ArrayList


    Public Sub New()
        'Here is stuff you want to happen when the page loads, so it doesn't have to when the function is called.

        'Now we load AppData, AppPath from Global.asax and instantiate it at the beginning of the Class, setting StarTime as well
        'AppData = BTPricingServices.Global_asax.MyAppData
        'AppPath = BTPricingServices.Global_asax.AppPath
        'StartTime = Now

    End Sub

    'Sub LoadAppData(ByVal apppath As String)

    '    Try
    '        'Note the name is hard coded because this is a web app 
    '        Dim AppData = XDocument.Load(apppath & "BTPricingServices" & "AppData.xml")
    '        Warehouses = New ArrayList
    '        StatusMessages = New ArrayList
    '        AccountIDTypes = New ArrayList
    '        ItemIDTypes = New ArrayList
    '        EmailRecipients = New ArrayList

    '        For Each WHS As XElement In AppData...<Warehouse>

    '            Dim WHSInfo As New Warehouse

    '            WHSInfo.Code = WHS.<Code>.Value
    '            WHSInfo.Description = WHS.<Description>.Value

    '            Warehouses.Add(WHSInfo)

    '        Next

    '        For Each StatMess As XElement In AppData...<StatusMessage>

    '            Dim StatusInfo As New StatusMessage

    '            StatusInfo.Code = StatMess.<Code>.Value
    '            StatusInfo.Description = StatMess.<Description>.Value

    '            StatusMessages.Add(StatusInfo)

    '        Next

    '        For Each Atype As XElement In AppData...<AccountIDType>
    '            Dim AcctInfo As New AccountIDType

    '            AcctInfo.Code = Atype.<Code>.Value
    '            AcctInfo.Description = Atype.<Description>.Value

    '            AccountIDTypes.Add(AcctInfo)

    '        Next

    '        For Each Itype As XElement In AppData...<ItemIDType>
    '            Dim ItemInfo As New ItemIDType

    '            ItemInfo.Code = Itype.<Code>.Value
    '            ItemInfo.Description = Itype.<Description>.Value

    '            ItemIDTypes.Add(ItemInfo)

    '        Next

    '        For Each Recipient As XElement In AppData...<EmailRecipient>

    '            Dim EmailJerk As New EmailRecipient
    '            EmailJerk.Name = Recipient.<Name>.Value
    '            EmailJerk.Address = Recipient.<EmailAddress>.Value
    '            EmailJerk.Group = Recipient.<Group>.Value

    '            EmailRecipients.Add(EmailJerk)

    '        Next


    '    Catch ex As Exception

    '        Dim errmess As String = "Terminating Due to Error Loading Application Data XML: " & ex.Message & " stack trace: " & ex.StackTrace
    '        'Direct Send Emergency Mail because we may not have an email distribution list
    '        '   The values used in EmergeMail are loaded from the appconfig.
    '        EmergeMail(errmess)


    '    End Try

    'End Sub

    Function AnalyzeSKU(ByVal ItemID As String) As SKUInfo
        'Note that this is used to set the type, but not for validation because we can't analyze every sku in a multiline request
        '   and reject all if 1 or more are bad... so if it's bad, it goes.  We do, however, rough validation for a single line call.

        Dim si As New SKUInfo

        Select Case Len(Trim(ItemID))
            'Note that since BTKey is 10 byte as stored in ODS, and 10 byte ISBNs are not allowed,
            '   this logic is different than in previous versions and assumes a 10 byte entry is the
            '   only valid BTKey length accepted.  No attempt is made to determine if it my be an ISBN10.

            Case Is = 10
                'BTKeys for BTE always start with "6", so in case I get a BTE BTKey (although I am not supposed to from NextGen)
                '   I'll go ahead and route it properly.
                If Left(ItemID, 1) = "6" Then
                    si.ItemID = ItemID
                    si.Source = "BTE"
                    si.Valid = True
                    si.ItemFlag = "B"
                    si.EANFlag = False
                Else
                    si.ItemID = ItemID
                    si.Source = "BTB"
                    si.Valid = True
                    si.ItemFlag = "B"
                    si.EANFlag = False
                End If
            Case Is = 11, 12
                si.ItemID = ItemID
                si.Source = "BTE"
                si.Valid = True
                si.ItemFlag = "U"
                si.EANFlag = False
            Case Is < 10
                si.ItemID = ItemID
                si.Source = "BTB"
                si.Valid = False
                si.ItemFlag = "B"
                si.EANFlag = False
            Case Is = 13
                si.EANFlag = True
                'Note that for this version, ISBNs are only used for BTE, BTB is strictly by BTKey
                If Left(ItemID, 3) = "978" Or Left(ItemID, 3) = "979" Then
                    si.ItemID = ItemID
                    si.Source = "BTE"
                    si.Valid = True
                    si.ItemFlag = "I"
                Else
                    si.ItemID = ItemID
                    si.Source = "BTE"
                    si.Valid = True
                    si.ItemFlag = "U"
                End If
            Case Is = 14
                si.EANFlag = True
                If Left(ItemID, 4) = "0978" Or Left(ItemID, 4) = "0979" Then
                    si.ItemID = ItemID
                    si.Source = "BTE"
                    si.Valid = True
                    si.ItemFlag = "I"
                Else
                    si.ItemID = ItemID
                    si.Source = "BTE"
                    si.Valid = True
                    si.ItemFlag = "U"
                End If
            Case Is > 14
                si.EANFlag = True
                si.ItemID = ItemID
                si.Source = "BTE"
                si.Valid = False
                si.ItemFlag = "U"
        End Select
        AnalyzeSKU = si
    End Function

    Function AnalyzeCusID(ByVal cusid As String) As CusIDInfo
        Dim ci As New CusIDInfo

        Select Case Len(Trim(cusid))

            'Note that Accounts can only be BTE (8 byte). CFCS (20 byte) or SANs are not allowed for this interface.
            Case Is < 7
                ci.CusNo = cusid
                ci.Type = "a length less than 7 bytes"
                ci.Valid = False
            Case Is = 7
                ci.CusNo = cusid
                ci.Type = "SAN"
                ci.Valid = False
            Case Is = 8
                ci.CusNo = cusid
                ci.Type = "BTE"
                ci.Valid = True
            Case Is = 9, 10, 11, 12
                ci.CusNo = cusid
                ci.Type = "SAN"
                ci.Valid = False
            Case Is = 13, 14, 15, 16, 17, 18, 19
                ci.CusNo = cusid
                ci.Type = "a length of 13 to 19 bytes"
                ci.Valid = False
            Case Is = 20
                ci.CusNo = cusid
                ci.Type = "CFCS"
                ci.Valid = False
            Case Is > 20
                ci.CusNo = cusid
                ci.Type = "a length greater than 20 bytes"
                ci.Valid = False

        End Select
        AnalyzeCusID = ci
    End Function

    Function GetStatusDescription(ByVal code As String) As String
        GetStatusDescription = code & ": No Description Found for this Code."

        For Each codepair As AppDataStatusMessage In AppData.StatusMessage
            If UCase(codepair.Code) = UCase(code) Then
                GetStatusDescription = code & ": " & codepair.Description
                Exit For
            End If
        Next

        If code.Length < 4 Then GetStatusDescription = "No Status Message Returned."

        Return GetStatusDescription

    End Function
    Function GetWarehouseDescription(ByVal code As String) As String
        GetWarehouseDescription = code & ": This WHS code not recognized!"

        For Each codepair As AppDataWarehouse In AppData.Warehouse
            If UCase(codepair.Code) = UCase(code) Then
                GetWarehouseDescription = codepair.Description
                Exit For
            End If
        Next

        Return GetWarehouseDescription

    End Function
    Function GetAccountIDTypeDescription(ByVal code As String) As String
        GetAccountIDTypeDescription = code & ": This AccountIDType not recognized!"

        For Each codepair As AppDataAccountIDType In AppData.AccountIDType
            If UCase(codepair.Code) = UCase(code) Then
                GetAccountIDTypeDescription = codepair.Description
                Exit For
            End If
        Next

        Return GetAccountIDTypeDescription

    End Function
    Function GetItemIDTypeDescription(ByVal code As String) As String
        GetItemIDTypeDescription = code & ": ItemIDType not recognized!"

        For Each codepair As AppDataItemIDType In AppData.ItemIDType
            If UCase(codepair.Code) = UCase(code) Then
                GetItemIDTypeDescription = codepair.Description
                Exit For
            End If
        Next

        Return GetItemIDTypeDescription

    End Function

    Sub EmergeMail(ByVal messtosend As String)

        If Not My.Settings.EnableEmails Then Exit Sub

        Dim SMTPFormTo As String = My.Settings.EmergeMailTo
        Dim SMTPServer As String = My.Settings.SMTPServer
        'This is for when we suspect Network Connectivity issues that might prevent use of MailMan.
        'it remains to be seen if we need to prevent hordes of emails with a "lastnotified" check.

        'If DateDiff(DateInterval.Minute, LastAlert, Now) < My.Settings.NotificationInterval Then Exit Sub

        Dim SMTPMess As New MailMessage("PageMan@baker-taylor.com", SMTPFormTo)
        Dim SMTPMail As New SmtpClient(SMTPServer)


        SMTPMess.Subject = "BTPricingServices EmergeMail."
        SMTPMess.Body = "BTPricingServices : " & messtosend


        SMTPMail.Send(SMTPMess)
        SMTPMess = Nothing
        SMTPMail = Nothing

        'LastAlert = Now

    End Sub
    Sub Loggit(Optional ByVal path As String = "", Optional ByVal system As String = "No system", Optional ByVal pass As String = "No pass", Optional ByVal custid As String = "No custid", Optional ByVal skucount As String = "No skucount", Optional ByVal mess As String = "no mess")

        If path = "" Then path = Context.Request.PhysicalApplicationPath


        'For now, we always do the simple 1 line per call log entry
        'This setting is in effect for the DebugIt logging only.
        'If Not My.Settings.EnableLogging Then Exit Sub

        Dim linemess As String = system & "," & pass & "," & custid & "," & skucount & "," & mess & "," & Now.ToString

        'I can't seem to use Format in an expression, so I break the date and padd it up here.
        Dim MM As String
        Dim DD As String

        MM = Today.Month.ToString
        If Len(MM) < 2 Then MM = "0" & MM
        DD = Today.Day.ToString
        If Len(DD) < 2 Then DD = "0" & DD


        Dim DSTamp As String = Today.Year.ToString & MM & DD

        Dim logger As System.IO.StreamWriter = Nothing
        Dim retries As Short = 0

tryagain:

        Try
            logger = My.Computer.FileSystem.OpenTextFileWriter(path & "Logs\" & DSTamp & "_Log.txt", True, Encoding.ASCII)
            logger.WriteLine(linemess)
            logger.Close()

        Catch ex As Exception
            'for now, we don't care if there are issues writing to the log file, but we'll try to close it
            If Not IsNothing(logger) Then logger.Close()
            retries += 1
            Thread.Sleep(100)
            If retries < 10 Then GoTo tryagain


        End Try
        logger = Nothing
        DSTamp = Nothing
        MM = Nothing
        DD = Nothing


    End Sub
    Sub DebugIt(Optional ByVal path As String = "", Optional ByVal mess As String = "No DebugIt Message.")

        If path = "" Then path = Context.Request.PhysicalApplicationPath

        If Not IsNothing(My.Settings.EnableLogging) Then
            If Not My.Settings.EnableLogging Then Exit Sub
        End If

        Dim linemess As String = mess & " - " & Now.ToString

        'I can't seem to use Format in an expression, so I break the date and padd it up here.
        Dim MM As String
        Dim DD As String

        MM = Today.Month.ToString
        If Len(MM) < 2 Then MM = "0" & MM
        DD = Today.Day.ToString
        If Len(DD) < 2 Then DD = "0" & DD


        Dim DSTamp As String = Today.Year.ToString & MM & DD

        Dim logger As System.IO.StreamWriter = Nothing
        Dim retries As Short = 0

tryagain:

        Try
            logger = My.Computer.FileSystem.OpenTextFileWriter(path & "Logs\" & DSTamp & "Debug_Log.txt", True, Encoding.ASCII)
            logger.WriteLine(linemess)
            logger.Close()

        Catch ex As Exception
            'for now, we don't care if there are issues writing to the log file, but we'll try to close it
            If Not IsNothing(logger) Then logger.Close()
            retries += 1
            Thread.Sleep(100)
            If retries < 10 Then GoTo tryagain


        End Try

        logger = Nothing
        DSTamp = Nothing
        MM = Nothing
        DD = Nothing



    End Sub
    Sub LogMess(Optional ByVal path As String = "", Optional ByVal linemess As String = "no message")

        If path = "" Then path = Context.Request.PhysicalApplicationPath

        'For now, we always do the simple 1 line per call log entry
        'This setting is in effect for the DebugIt logging only.
        'If Not My.Settings.EnableLogging Then Exit Sub

        linemess = linemess & "," & Now.ToString

        'I can't seem to use Format in an expression, so I break the date and padd it up here.
        Dim MM As String
        Dim DD As String

        MM = Today.Month.ToString
        If Len(MM) < 2 Then MM = "0" & MM
        DD = Today.Day.ToString
        If Len(DD) < 2 Then DD = "0" & DD


        Dim DSTamp As String = Today.Year.ToString & MM & DD

        Dim logger As System.IO.StreamWriter = Nothing
        Dim retries As Short = 0

tryagain:

        Try
            logger = My.Computer.FileSystem.OpenTextFileWriter(path & "Logs\" & DSTamp & "_Log.txt", True, Encoding.ASCII)
            logger.WriteLine(linemess)
            logger.Close()

        Catch ex As Exception
            'for now, we don't care if there are issues writing to the log file, but we'll try to close it
            If Not IsNothing(logger) Then logger.Close()
            retries += 1
            Thread.Sleep(100)
            If retries < 10 Then GoTo tryagain

        End Try

        logger = Nothing
        DSTamp = Nothing
        MM = Nothing
        DD = Nothing


    End Sub

    <WebMethod(Description:="Tolas Pricing Request for a Single Item by UPC or BTKey (with details) and AccountID")> _
    Public Function PricingRequestByParameters(ByVal SystemID As String, ByVal SystemPassword As String, ByVal AccountID As String, ByVal WHSCode As String, ByVal ItemID As String, ByVal Quantity As String, ByVal ReturnFlag As String, ByVal PriceKey As String, ByVal ProductLine As String, ByVal ListPrice As String, ByVal AcceptableDiscount As String) As PricingResponse

        PricingRequestByParameters = New PricingResponse
        PricingRequestByParameters.HeaderMessage = "Initialized"

        Dim Request As New PricingRequest

        Try
            If IsNothing(StartTime) Then StartTime = Now
            'StartTime = Now

            'Dim AppPath As String = Context.Request.PhysicalApplicationPath
            Dim SexE As String = GetElapsed()

            'In order to exercise this service using manual parameter entry,
            '   a single item can be priced by entering the details for the entire class
            '   when it only consists of 1 SKU.  This function turns it into the Class
            '   that is then passed on to the core function.

            If Trim(Quantity) = "" Then Quantity = "1"
            If Trim(ListPrice) = "" Then ListPrice = "0.0"
            If Trim(AcceptableDiscount) = "" Then AcceptableDiscount = "0.0"



            Request.SystemID = SystemID
            Request.SystemPassword = SystemPassword
            Request.AccountID = AccountID
            Request.WHSCOD = WHSCode

            Request.ItemList = New List(Of ItemRequestDetail)

            Dim Item As New ItemRequestDetail
            Item.ItemId = ItemID
            Item.Quantity = Quantity
            Item.ReturnFlag = ReturnFlag
            Item.PriceKey = PriceKey
            Item.ProductLine = ProductLine
            Item.ListPrice = ListPrice
            Item.AcceptableDiscount = AcceptableDiscount

            Request.ItemList.Add(Item)


            PricingRequestByParameters = GetTolasPricing(Request)

            Dim fullretries As Short = 0

            Do Until fullretries > 3
                If InStr(PricingRequestByParameters.HeaderMessage, "Errors Encountered: - Please Resend.") > 0 Then
                    fullretries += 1
                    PricingRequestByParameters = GetTolasPricing(Request)
                Else
                    Exit Do
                End If

            Loop

            If fullretries > 3 Then
                'get the physical path where we live.
                'Dim AppPath As String = Context.Request.PhysicalApplicationPath
                DebugIt(AppPath, "============== " & "No Data Available Errors after 3 full retries. Giving up.: " & PricingRequestByParameters.HeaderMessage)
                Loggit(AppPath, Request.SystemID, Request.SystemPassword, Request.AccountID, Request.ItemList.Count.ToString, " Status: " & PricingRequestByParameters.HeaderMessage)
            End If

            SexE = GetElapsed()
            DebugIt(AppPath, "        " & PricingRequestByParameters.HeaderMessage & " Single Unit By Parameters. Elapsed Secs= " & SexE)

            SexE = GetElapsed()
            Loggit(AppPath, Request.SystemID, Request.SystemPassword, Request.AccountID, Request.ItemList.Count.ToString, " Status: " & PricingRequestByParameters.HeaderMessage & " Elapsed Secs= " & SexE)


        Catch ex As Exception
            DebugIt(AppPath, "============== " & "Unanticipated Exception Encountered in PricingRequestParameter. Will attempt to Loggit. " & ex.Message & " Response.HeaderMessage = " & PricingRequestByParameters.HeaderMessage & " Stack Trace: " & ex.StackTrace)
            Try
                Loggit(AppPath, SystemID, SystemPassword, AccountID, " 1 Exception: " & ex.Message)

            Catch exi As Exception
                'DebugIt(AppPath, "============== Couldn't Loggit: " & exi.Message)
                LogMess(AppPath, "RequestByParameters problems: " & PricingRequestByParameters.HeaderMessage & " Exceptions:" & ex.Message & " ...and Loggit Exception: " & exi.Message)
            End Try


        End Try


        Return PricingRequestByParameters

    End Function


    <WebMethod(Description:="Pricing by RequestClass")> _
    Public Function PricingRequestByClass(ByVal RequestClass As PricingRequest) As PricingResponse

        PricingRequestByClass = New PricingResponse
        PricingRequestByClass.HeaderMessage = "Initialized"

        If IsNothing(RequestClass) Then PricingRequestByClass.HeaderMessage = PricingRequestByClass.HeaderMessage + "...but RequestClass is Empty"

        Try
            If IsNothing(StartTime) Then StartTime = Now
            'StartTime = Now
            'Dim AppPath As String = Context.Request.PhysicalApplicationPath
            Dim SexE As String = GetElapsed()

            'In order to allow multiple paths to the core functionality, this is exposed to the outside world,
            '   but simply passes the class over to the GetTolasPricing Function that handles the actual work.
            PricingRequestByClass = GetTolasPricing(RequestClass)

            Dim fullretries As Short = 0

            Do Until fullretries > 3
                If InStr(PricingRequestByClass.HeaderMessage, "Errors Encountered: - Please Resend.") > 0 Then
                    fullretries += 1
                    PricingRequestByClass = GetTolasPricing(RequestClass)
                Else
                    Exit Do
                End If

            Loop

            If fullretries > 3 Then
                'get the physical path where we live.
                'Dim AppPath As String = Context.Request.PhysicalApplicationPath
                DebugIt(AppPath, "        " & "No Data Available Errors after 3 full retries. Giving up.: " & PricingRequestByClass.HeaderMessage)
                Loggit(AppPath, RequestClass.SystemID, RequestClass.SystemPassword, RequestClass.AccountID, RequestClass.ItemList.Count.ToString, " Status: " & PricingRequestByClass.HeaderMessage)

            End If

            SexE = GetElapsed()
            DebugIt(AppPath, "        " & PricingRequestByClass.HeaderMessage & " SKUs=" & RequestClass.ItemList.Count.ToString & " Elapsed Secs= " & SexE)

            SexE = GetElapsed()
            Loggit(AppPath, RequestClass.SystemID, RequestClass.SystemPassword, RequestClass.AccountID, RequestClass.ItemList.Count.ToString, " Status: " & PricingRequestByClass.HeaderMessage & " Elapsed Secs= " & SexE)


        Catch ex As Exception
            DebugIt(AppPath, "        " & "Unanticipated Exception Encountered in PricingRequestByClass. Will attempt to Loggit. " & ex.Message & " Response.HeaderMessage = " & PricingRequestByClass.HeaderMessage & " StackTrace: " & ex.StackTrace)
            Try
                Loggit(AppPath, RequestClass.SystemID, RequestClass.SystemPassword, RequestClass.AccountID, RequestClass.ItemList.Count.ToString, " Exception: " & ex.Message)

            Catch exi As Exception
                'DebugIt(AppPath, "        Couldn't Loggit: " & exi.Message)
                LogMess(AppPath, "RequestByClass problems: " & PricingRequestByClass.HeaderMessage & " Exceptions:" & ex.Message & " ...and Loggit Exception= " & exi.Message)
            End Try

        End Try

        Return PricingRequestByClass

    End Function


    Public Function GetTolasPricing(ByVal Request As PricingRequest) As PricingResponse

        'get the physical path where we live.
        'Dim AppPath As String = Context.Request.PhysicalApplicationPath

        Dim SexE As String = GetElapsed()
        DebugIt(AppPath, "        " & "Starting GetTolasPricing." & " Elapsed Secs= " & SexE)

        'Created, but empty until it is filled in with the working 'Response' object of the same type.
        GetTolasPricing = New PricingResponse
        GetTolasPricing.HeaderMessage = "Initialized"
        If IsNothing(Request) Then GetTolasPricing.HeaderMessage = GetTolasPricing.HeaderMessage & "...but GetTolasPricing Request object is empty."

        'Inbound Meat with Listener Call(s).
        'This function is where the work is done, but it is not exposed to the outside so that the
        '   WebMethods can each call this.  That way, the WebMethods can take different input structures,
        '   but function the same way with common code.

        'If Request.SystemID = "" Then
        '    Request.SystemID = "NEXTGEN"
        '    Request.SystemPassword = "N@xtG3n"
        '    Request.AccountID = "12345678"
        '    Request.WHSCOD = "SOM"
        'End If

        Try
            'we'll use a class in between to define the parsed response...
            Dim Response As New PricingResponse

            Response.AccountID = Request.AccountID
            Response.ItemCount = Request.ItemList.Count
            Response.WHSCOD = Request.WHSCOD
            Response.HeaderMessage = "Submitting Request..."


            'go get the Application Variables from web.config...
            Dim BTEIPPort As String = ""
            Dim BTEIPAddr As String = ""
            Dim IPTimeout As Short = 0
            Dim PassCode As String = ""
            Dim MaxItems As Short = 0

            Try
                BTEIPPort = My.Settings.BTEIPPort
                BTEIPAddr = My.Settings.BTEIPAddr
                IPTimeout = My.Settings.IPTimeOut
                MaxItems = My.Settings.MaxDetailsToTolas

                PassCode = My.Settings.PassCode


            Catch ex As Exception

                Response.HeaderMessage = "Error Getting Configuation Settings. Contact Web Service Developer: " & ex.Message
                GetTolasPricing = Response
                Loggit(AppPath, Request.SystemID, Request.SystemPassword, Request.AccountID, Request.ItemList.Count.ToString, Response.HeaderMessage)
                EmergeMail(Response.HeaderMessage & ex.Message)
                DebugIt(AppPath, "Error Response: " & Response.HeaderMessage)
                Return Response

            End Try


            'Start by validating the header elements and confirming there is at least 1 item to do.
            '   Note that the item related statements have been commented out.  They will not be included in the basic validation
            '   but instead will be handled while processing the lines.  No single bad line item should abort the response, unless the
            '   request itself is a single line request.

            Dim PassKey As String
            PassKey = "*" + Trim(Request.SystemID) + Trim(Request.SystemPassword) + "*"
            'surround the sysid and password with *'s so entering a partial match does not give access.
            'the PassCode is in the form of *useridpassword*useridpassword*useridpassword*....etc.
            Select Case InStr(PassCode, PassKey)
                Case Is > 0
                    'you may pass
                Case Else
                    Response.HeaderMessage = "Access Denied. Invalid SystemID and SystemPassword Pairing."
                    GetTolasPricing = Response
                    Loggit(AppPath, Request.SystemID, Request.SystemPassword, Request.AccountID, Request.ItemList.Count.ToString, Response.HeaderMessage)
                    DebugIt(AppPath, "Error Response: " & Response.HeaderMessage)
                    Return Response
            End Select

            'you passed, so setup the defaults
            'StockResponse.ItemID = ItemID 'this used to be what came back from legacy literally. Now it is what they sent us.


            'let's make sure legacy systems aren't confused by lower case alphas
            Request.AccountID = UCase(Request.AccountID)

            'first, see if the Customer submitted is valid (a BTE 8-byte number)
            Dim ci As New CusIDInfo
            Dim inputok As Boolean
            Dim invalidInputMess As String = ""



            'first we'll assume everything is ok...
            inputok = True
            ci = AnalyzeCusID(Request.AccountID)

            If Not ci.Valid Then
                invalidInputMess = "Account Number not proper format for Tolas: " & Request.AccountID & " = " & ci.Type
                inputok = False
            End If

            If Request.ItemList.Count < 1 Then
                invalidInputMess = invalidInputMess & " No Line Items included in Request Class."
                inputok = False
            End If

            If Request.ItemList.Count > 9999 Then
                invalidInputMess = invalidInputMess & " Illegal Request.  LineItem Count cannot exceed 9999."
                inputok = False
            End If

            If InStr("COM*SOM*MOM*RNO*IND*VIE*VIM", UCase(Request.WHSCOD)) < 1 Then
                invalidInputMess = invalidInputMess & " Invalid WHS Code: " & Request.WHSCOD
                inputok = False
            End If

            If Not inputok Then
                Response.HeaderMessage = "Error: " & invalidInputMess
                Loggit(AppPath, Request.SystemID, Request.SystemPassword, Request.AccountID, Request.ItemList.Count.ToString, Response.HeaderMessage)
                DebugIt(AppPath, "Error Response: " & Response.HeaderMessage)
                Return Response
            End If

            'It looks like we have a valid request, so let's prepare to build the TCPIP request/response elements.
            SexE = GetElapsed()
            DebugIt(AppPath, "Creating Send Strings." & " Elapsed Secs= " & SexE)

            Dim SendHeaderStr As New IO.StringWriter
            Dim SendLinesStr As New IO.StringWriter


            Dim hostaddr As String = ""
            Dim hostport As Integer = 0
            'Dim transno As String = "XXXXXXXXXXXXXXXXXXXX" '20 bytes to be made up by me.  The logic will come a little later. 
            Dim transno As String = GetHexName(12) '20 bytes to be made up by me.  This is just the base number to use.  Note that it won't necessarily be unique.


            hostaddr = Trim(BTEIPAddr)
            hostport = Val(Trim(BTEIPPort))


            'First, construct the request string, starting with a header and appending all the line items... up to the limit

            'Setup for making mulitple calls as we spin through the lines.
            'Response = New PricingResponse
            'by setting this to false, we ensure that a full response is created if the maxlines threshold is never reached.
            Dim appendonly As Boolean = False
            Dim linetally As Short = 0
            Dim TempRecvStr As String = ""
            Dim tolascalls As Short = 1

            'calculate the number of full Tolas Calls to be made...
            Dim calccalls As Integer = Int(Request.ItemList.Count / My.Settings.MaxDetailsToTolas)

            '...and if there are lines leftover, add one more call to the calculated calls
            If Request.ItemList.Count Mod My.Settings.MaxDetailsToTolas > 0 Then calccalls += 1

            For Each LineItem As ItemRequestDetail In Request.ItemList
                'OK, I think we can just break things up dynamically while we do the adding of all the lines.
                Dim si As SKUInfo
                si = AnalyzeSKU(LineItem.ItemId)

                If si.Valid = False And Request.ItemList.Count = 1 Then
                    'for a single line call, we try to validate the itemid before calling tolas and abort if it is bad.
                    SendHeaderStr.Close()
                    SendLinesStr.Close()
                    Response.HeaderMessage = "Error: Invalid SKU on Single Line Call. SKU = [" & si.ItemID & "] of Type = [" & si.ItemFlag & "]"
                    Loggit(AppPath, Request.SystemID, Request.SystemPassword, Request.AccountID, Request.ItemList.Count.ToString, Response.HeaderMessage)
                    DebugIt(AppPath, "Error Response: " & Response.HeaderMessage)
                    'I'm not sure if returning from inside a foreach is a problem or not, but I don't think since we are a one shot call.
                    Return Response
                End If


                SendLinesStr.Write(si.ItemFlag)
                SendLinesStr.Write(Mid(LineItem.ItemId & "              ", 1, 14))
                SendLinesStr.Write(Format(LineItem.Quantity, "00000000"))
                SendLinesStr.Write(Mid(LineItem.ReturnFlag & " ", 1, 1))
                SendLinesStr.Write(Mid(LineItem.PriceKey & " ", 1, 1))
                SendLinesStr.Write(Mid(LineItem.ProductLine & "      ", 1, 6))
                SendLinesStr.Write(Format(LineItem.ListPrice * 100, "00000000"))
                SendLinesStr.Write(Format(LineItem.AcceptableDiscount * 100, "00000"))

                linetally += 1

                'put a counter above, increment it here, and when we hit the max lines...
                If linetally = My.Settings.MaxDetailsToTolas Then
                    'Create a new header before each call, reflecting the lines to be sent
                    SendHeaderStr.Close()
                    SendHeaderStr = New IO.StringWriter
                    SendHeaderStr.Write(PadIt(transno & "_" & Trim(tolascalls.ToString) & "of" & Trim(calccalls.ToString), " ", 20, "l"))
                    SendHeaderStr.Write(Request.AccountID)
                    SendHeaderStr.Write(Request.WHSCOD)
                    'this will actually be a maximum of whatever the Tolas limit is when the itemlist.count is over that limit.
                    '   that logic will come a little later.
                    SendHeaderStr.Write(Format(linetally, "00000000"))
                    SexE = GetElapsed()
                    DebugIt(AppPath, SendHeaderStr.ToString & " Elapsed Secs= " & SexE)

                    'Assign the response string to the Function that calls the listener
                    '   We do the CallListener
                    TempRecvStr = CallListener(hostaddr, hostport, SendHeaderStr.ToString & SendLinesStr.ToString, My.Settings.TCPIPBufferSize)
                    'DebugIt(AppPath, TempRecvStr)


                    '   Parse the Response...
                    '   ...but only add the lines after the first execution
                    'ParseResponse(RecvStr, Response, appendonly)
                    If appendonly Then
                        RecvStr = RecvStr & Mid(Trim(TempRecvStr), 44, 32 * linetally)
                    Else
                        RecvStr = Mid(Trim(TempRecvStr), 1, Trim(TempRecvStr).Length)
                    End If
                    'reset the tally counter to zero, along with the collection of lines
                    linetally = 0
                    SendLinesStr.Close()
                    SendLinesStr = New IO.StringWriter

                    '...and put us in append mode from here on out.
                    appendonly = True
                    tolascalls += 1
                End If
                '
            Next
            'there is a slim chance that the lines will be an exact multiple of the maxlines value,
            '   so to avoid making a line-less call, we just check the line tally count one more time.
            '   ...which also will fire this for any request under the maxlines value.
            If linetally > 0 Then
                'DebugIt(AppPath, SendHeaderStr.ToString)
                'DebugIt(AppPath, SendLinesStr.ToString)

                'Create a new Header for each call
                SendHeaderStr.Close()
                SendHeaderStr = New IO.StringWriter
                SendHeaderStr.Write(PadIt(transno & "_" & Trim(tolascalls.ToString) & "of" & Trim(calccalls.ToString), " ", 20, "l"))
                SendHeaderStr.Write(Request.AccountID)
                SendHeaderStr.Write(Request.WHSCOD)
                'this will actually be a maximum of whatever the Tolas limit is when the itemlist.count is over that limit.
                '   that logic will come a little later.
                SendHeaderStr.Write(Format(linetally, "00000000"))
                SexE = GetElapsed()
                DebugIt(AppPath, SendHeaderStr.ToString & " Elapsed Secs= " & SexE)

                'Assign the response string to the Function that calls the listener
                '   We do the CallListener
                TempRecvStr = CallListener(hostaddr, hostport, SendHeaderStr.ToString & SendLinesStr.ToString, My.Settings.TCPIPBufferSize)
                'DebugIt(AppPath, TempRecvStr)
                If appendonly Then
                    'do some math to figure out exactly what this SHOuLD be, making sure you drop the trailing chr(3)
                    '   Then maybe you can add it all back in above.
                    RecvStr = RecvStr & Mid(Trim(TempRecvStr), 44, 32 * linetally)
                Else
                    RecvStr = Mid(Trim(TempRecvStr), 1, Trim(TempRecvStr).Length)
                End If

                '   Parse the Response...
                '   ...but only add the lines after the first execution
            End If

            'DebugIt(AppPath, RecvStr)
            SexE = GetElapsed()
            DebugIt(AppPath, "All Responses Recieved.  Parsing..." & " Elapsed Secs= " & SexE)

            'If the string "Errors - Please Retry" is in there anywhere, we have an invalid response, but we want to try again
            '   So just send a special message back to the internal caller to try again.
            If InStr(UCase(RecvStr), "PLEASE RETRY") > 0 Then
                Response.HeaderMessage = "Errors Encountered: - Please Resend." & Response.HeaderMessage
                DebugIt(AppPath, "Error Response: " & Response.HeaderMessage)
                Return Response
            End If

            'If the word Error is in there anywhere, we have an invalid response
            '   So just append the whole response to the Header Message, after pre-pending "Errors Encountered: "
            If InStr(UCase(RecvStr), "ERROR") > 0 Then
                Response.HeaderMessage = "Errors Encountered: " & Response.HeaderMessage & RecvStr
                DebugIt(AppPath, "Error Response: " & Response.HeaderMessage)
                Return Response
            End If


            Response = ParseResponse(RecvStr, Request.ItemList.Count)

            'Now we should have the complete Response.Object.
            'If I haven't done a Return before this point, you made it without exceptions.  
            '   Here is your response
            'Loggit(AppPath, Request.SystemID, Request.SystemPassword, Request.AccountID, Request.WHSCOD, " Sent: " & SendHeaderStr.ToString & SendLinesStr.ToString & " Got: " & RecvStr & " Status: " & Response.HeaderMessage)
            GetTolasPricing = Response

            SexE = GetElapsed()
            DebugIt(AppPath, "Parse Complete.  Returning XML response.  GetTolasPricing Complete." & " Elapsed Secs= " & SexE)


        Catch ex As Exception
            DebugIt(AppPath, "Unanticipated Exception Encountered during GetTolasPricing. " & ex.Message & " Will attempt to Loggit. StackTrace: " & ex.StackTrace)
            Try
                Loggit(AppPath, Request.SystemID, Request.SystemPassword, Request.AccountID, Request.ItemList.Count.ToString, " Exception: " & ex.Message)
            Catch exi As Exception
                'DebugIt(AppPath, "Couldn't Loggit: " & exi.Message)
                LogMess(AppPath, "GetTolasPricing problems: " & GetTolasPricing.HeaderMessage & " Exceptions:" & ex.Message & " ...and Loggit Exception: " & exi.Message)
            End Try

        End Try

        Return GetTolasPricing

    End Function
    Function CallListener(ByVal server As String, ByVal port As Integer, ByVal request As String, Optional ByVal buffersize As Integer = 1024) As String

        Dim linekey As String = "1"
        'get the physical path where we live.
        'Dim AppPath As String = Context.Request.PhysicalApplicationPath


        If My.Settings.EOTSendUsed Then request = request & Chr(My.Settings.EOTSendChar)

        Dim SexE As String = GetElapsed()
        DebugIt(AppPath, "CallListener: Started.  Sending " & request.Length.ToString & " bytes." & " Elapsed Secs= " & SexE)
        'DebugIt(AppPath, request)

        'Note that this code was adapted from a Windows Form App, hence some references to non-existent
        '   objects as well as the Application object.
        '
        'Also, for this multiline call we only take the first 39 bytes of the request here, which should be the header only.
        '   Other than that change, this is exactly as the code appears in BTStockCheck.
        CallListener = "Initializing... call to " & server & ":" & port.ToString & " buffersize:" & buffersize.ToString & ". Requesting '" & Mid(request, 1, 39) & "' (in between single quotes) of " & Len(request).ToString & " bytes."


        Dim StartSendTime As Date = Now
        Dim StartRecTime As Date    'This also doubles as end send time
        Dim EndRecTime As Date
        If buffersize < 1 Then
            CallListener = CallListener & " Error - Bad Buffer Size."
            Return CallListener
        End If

        Try
            ' Create a TcpClient.
            ' Note, for this client to work you need to have a TcpServer 
            ' connected to the same address as specified by the server, port
            ' combination.
            Dim client As New TcpClient(server, port)

            'Per GSN, adjusted these properties of the TcpClient:
            client.SendTimeout = 60000        'default is no timeout. arbitrarily set to 30 seconds
            client.ReceiveTimeout = 120000     'same as above, but set to 60 seconds
            client.NoDelay = True             'This means start sending data immediately, don't collect it. default is false

            linekey = "2"
            ' Translate the passed message into ASCII and store it as a Byte array.
            Dim data As [Byte]() = System.Text.Encoding.ASCII.GetBytes(request)

            linekey = "3"

            ' Get a client stream for reading and writing.
            '  Stream stream = client.GetStream();
            Dim stream As NetworkStream = client.GetStream()

            linekey = "4"
            ' Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length)

            StartRecTime = Now
            SexE = GetElapsed()
            DebugIt(AppPath, "CallListener: Request Sent." & " Elapsed Secs= " & SexE)

            linekey = "5"
            ' Receive the TcpServer.response.
            ' Buffer to store the response bytes.
            data = New [Byte](buffersize) {}

            ' String to store the response ASCII representation.
            Dim responseData As StringBuilder = New StringBuilder


            ' Read the first batch of the TcpServer response bytes.
            Dim BytesRead As Int32 = 0
            Dim readcount As Short = 1

            Dim Done As Boolean = False

            Dim lcnt As Short = 0
            Do Until stream.DataAvailable
                If stream.DataAvailable Then Exit Do

                Thread.Sleep(250)
                lcnt += 1
                If lcnt > 40 Then

                    'we've waited 5 seconds for the first data to begin to arrive, so something isn't right
                    '   and we'll construct and return a special message to trigger a resend of the entire request.
                    CallListener = CallListener & " Error - Please Retry. No Data Available for " & (lcnt / 4).ToString & "seconds."
                    DebugIt(AppPath, CallListener)
                    Done = True
                    Return CallListener

                End If
                'remove after testing
                'DebugIt(AppPath, "...waiting " & lcnt.ToString & " of 40 quarter seconds for data....")

            Loop

            SexE = GetElapsed()
            DebugIt(AppPath, "CallListener: Data Arriving..." & " Elapsed Secs= " & SexE)

            Do
                linekey = "doloop"
                'Read
                BytesRead = stream.Read(data, 0, data.Length)
                'Accumulate
                responseData.AppendFormat("{0}", System.Text.Encoding.ASCII.GetString(data, 0, BytesRead))

                linekey = "done?"

                'Make a decison about doneness
                If Not My.Settings.EOTRcvUsed Then
                    'if we aren't using an EndOfTransmission marker, then we'll use the less reliable and more costly
                    '   wait around for data to become available.  If it isn't available after we wait, we're done.
                    linekey = "ifwait"

                    If My.Settings.Wait4It Then
                        If Not stream.DataAvailable Then
                            If Not client.Connected Then
                                'txtReadsPerformed.Text = "Disconnected after number of reads reached " & readcount.ToString
                                Done = True
                            End If
                            'txtReceiveStatus.Text = "...Waiting For More Data Available after block " & readcount.ToString
                            'Application.DoEvents()
                            Threading.Thread.Sleep(My.Settings.WaitMSex)
                        End If
                        If Not stream.DataAvailable Then Done = True
                    End If
                Else
                    linekey = "else"

                    'but if we do have a marker to look for, we just keep checking for it
                    '   and when we find it we're done.
                    '   Note: we are checking the whole response each time, it might be more
                    '       efficient and elegant to just check the most current read.
                    Done = responseData.ToString.Contains(Chr(My.Settings.EOTRcvChar))
                End If
                linekey = "outelse"

                readcount = readcount + 1
                'this Short overflows quickly if there is no EOT marker, so depending on what you want to do
                '   you can either call it done after a certain amount, or bomb it.  For now, we are just saying
                '   after 100 reads, you ain't getting nuttin more.
                If readcount > 100 Then Done = True

            Loop While Not Done

            linekey = "outloop"
            SexE = GetElapsed()
            DebugIt(AppPath, "CallListener: Ending.  Received: " & Len(responseData.ToString).ToString & " Bytes." & " Elapsed Secs= " & SexE)
            'DebugIt(AppPath, responseData.ToString)


            ' Close everything.
            stream.Close()
            client.Close()

            CallListener = responseData.ToString

        Catch e As ArgumentNullException
            CallListener = CallListener & " Error - ArgumentNullException " & e.Message

        Catch e As SocketException
            CallListener = CallListener & " Error - SocketException:  " & e.Message

        Catch ex As Exception
            CallListener = CallListener & " Error - Some Other Do-do happened: " & ex.Message & linekey

        End Try
        EndRecTime = Now

        Return CallListener

    End Function

    Function ParseResponse(ByVal rawresponse As String, ByVal totallineoverride As Integer) As PricingResponse

        'Note that since this may be a result of several calls to the Tolas Listener, the PricingResponse in its current state is passed into this function
        '   along with a flag that says whether the header portion should be created or just the lines appended.  That way, the same parsing code can be
        '   used regardless of whether or not this is a multi-call request... that is if this works.
        ParseResponse = New PricingResponse

        'Outbound Meat.


        Dim rawhead As String = Mid(rawresponse, 1, 43)
        Dim rawdetail As String = ""


        ParseResponse.HeaderMessage = GetStatusDescription(Mid(rawhead, 21, 4))
        ParseResponse.AccountID = Mid(rawhead, 25, 8)
        ParseResponse.WHSCOD = Mid(rawhead, 33, 3)
        'ParseResponse.ItemCount = Val(Mid(rawhead, 36, 8))
        ParseResponse.ItemCount = totallineoverride
        ParseResponse.ItemResponseDetails = New List(Of ItemResponseDetail)



        For x As Integer = 1 To ParseResponse.ItemCount
            rawdetail = Mid(rawresponse, 44 + (32 * (x - 1)), 32)

            'If Len(rawdetail) < 32 Then Continue For

            Dim DetailRec As New ItemResponseDetail
            DetailRec.DetailMessage = GetStatusDescription(Mid(rawdetail, 1, 4))

            DetailRec.ItemType = Trim(Mid(rawdetail, 5, 1))
            DetailRec.ItemID = Trim(Mid(rawdetail, 6, 14))
            DetailRec.DiscountPercent = Val(Mid(rawdetail, 20, 5)) / 100
            DetailRec.DiscountPrice = Val(Mid(rawdetail, 25, 8)) / 100

            ParseResponse.ItemResponseDetails.Add(DetailRec)

        Next

    End Function
    Function GetElapsed() As String

        Dim Elapsed As TimeSpan = Now - StartTime
        Dim SexElapsed As Decimal = Elapsed.Seconds + (Elapsed.Milliseconds / 1000)
        GetElapsed = Format(SexElapsed, "####0.000")

    End Function
    Function GetHexName(ByVal ln As Short) As String

        'GetHexName converts the current datetime into a string to create a unique filename
        'the first 8 bytes are hex representation of the date(1-4) and time (5-8) down to the second.
        'if the request is for a longer filename, the fractions of a second are appended up to 7 bytes.
        'if the request is for longer than 15 (8 + 7), zeros are appended.
        'using 8 bytes is only unique down to the second.

 

        Dim Htime As String
        Dim Hdate As String
        Dim RightNow As Date = Now
        Dim leftbyte As String
        Dim leftasc As Short
        Dim rightbytes As String


        Hdate = PadIt(Hex(DateValue(CStr(RightNow)).ToOADate), "0", 4, "r")
        Htime = PadIt(Hex(CDec(TimeValue(RightNow).ToOADate) * 100000), "0", 4, "r")

        If CDec(TimeValue(RightNow).ToOADate) > 0.65535 Then

            'our 4 byte time is now over hex FFFF, so we need to convert to "pseudo extended Hex" to avoid duplicate stamps
            'First, find the left byte of the hex time string
            leftbyte = Left(Htime, 1)

            'save the right 3 bytes of the hext time string
            rightbytes = Right(Htime, 3)

            'find the ACII value
            leftasc = Asc(leftbyte)

            'if it is numeric, we add 23 so that zero converts to "G", 1 to "H", etc...
            'but if it is "A" or above we add 16 so that "A" converts to "Q", etc...
            'this really isn't needed for time, because we will never get back up to Alpha values by midnight...
            'it is just here for reference.
            If leftasc < 58 Then leftasc = leftasc + 23 Else leftasc = leftasc + 16
            leftbyte$ = Chr(leftasc)

            'then we reassemble
            Htime$ = leftbyte + rightbytes

        End If


        GetHexName = Right(Trim(Hdate) & Trim(Htime), ln)

        If ln > 8 Then
            'fffffff means 7 digit fractional second. left justify and pad with zeros to append to 8 byte "Hex" name.
            '...this means you can actually get a unique filename down to a 10 millionth of a second... we think.
            '...and GetHexName(11) would give it down to thousandts.
            GetHexName = GetHexName + PadIt(RightNow.ToString("fffffff"), "0", ln - 8, "l")
        End If

    End Function
    Function PadIt(ByVal strtopad As String, ByVal pad As String, ByVal lng As Short, ByVal jus As String) As String

        PadIt = strtopad

        Dim padding As String = ""
        Dim x As Short

        For x = 1 To lng
            padding = padding & pad
        Next

        If UCase(jus) = "R" Then PadIt = Right(padding & strtopad, lng)
        If UCase(jus) = "L" Then PadIt = Left(strtopad & padding, lng)

    End Function

End Class