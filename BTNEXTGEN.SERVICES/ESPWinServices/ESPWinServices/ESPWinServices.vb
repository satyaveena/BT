'ESPWinServices
'
'New Windows Service that will be the long running basis for handling ESP related functions for TS360
'
'Author - Marvin Perkins
'
'Coding Started: 02-14-2014
'
'First Full Function Testing: 03-07-2014 - Pending ESP RESTful Service.
'
'Production Deployment:
'
'
'Notes:
'
'02-18-2014
'
'   Started working the basic functionality in JSON Demo Solution in order to get Windows Form Instant Feedback, etc.,
'   where I had previously created and consumed a RESTFul WCF Service inclusing of posting JSON, consuming JSON, and 
'   enabling both JSON or XML, including the passing of Arrays of Data.
'
'   Looked up and found that there is a Json Serializer/Deserializer within the System.Runtime.Serialization.Json namespace.
'   As usual, a Reference needed to be made to ...Serialization namespace before I could Import the .json underneath.  Additional
'   Note to Self: DON'T FORGET TO PUT THE REFERENCE IN THE CORRECt PROJECT OF YOUR SOLUTION! - duh.
'
'   Conecntrating on the Deserializer, was able to get Json deserialized into a .NET Class quite predictably.  Of course, the
'   Class names/properties, etc., must match exactly in case the JSon that is being consumed.  Also, the Arrays, Children, sub Arrays,
'   etc., must be structured in the class the same way they are presented in the JSon.  I spent a great deal of time getting the Class
'   to actually populate from the json, and it was all due to being able to properly translate the expected json to a Class.
'
'   Once complete (and I created a URI for my own RESTful WCF that actually returned the exact same json that esp creates), I copied
'   the code from the Win App to here in the Service.  I will enable Logging and Event Logging in much the same way as AutoFTPWinSvc.
'
'   The above was mostly from yesterday, which ended on a sour note when I could not completely install the Windows Service locally.
'   It did not seem that the new InstallShield LE tool "understood" that this was a Windows Service.  Had to spend the evening with 
'   that looming.
'
'   However, some searching uncovered a particular flag in the "Primary Output" > Properties area (.NET tab, "Installer" checkbox)
'   that handled this and resolved the issue.  Two additional problems with the new package are Warnings about Including .NET
'   Framework, but no proper instructions on how to do so and defaulting to a ServiceName of "Service1" without any way to override <<<<<<<<<<<< SEE 03-12-2014 Entry for the Resolution to Service1!
'   this.  It should be read from the ServiceInstaller module in the Service Project, but apparently it is not.  The new InstallShield LE
'   pachage is cumbersome, non-intuitive and buggy so far, not to mention how much clutter the "non-available to LE" options create.
'
'   Anyway, got it all installed, added some logging, a timer and some other stuff in the settings file so as to be able to start testing.
'   Now I can call the esp service directly, consume the json, and deserialize it (as evidenced by logging the ouput of the class, library
'   by library), and I only had to make one minor adjustment to match case ("ID" vs "Id") between the json and the Class.
'
'   Now, I am ready to turn towards SQL to get/set the last Fetch date (right now, I just use Now), and to finally pass the udt to
'   the new SP for updating the Libraries.
'
'   Excellent conclusion to the day.  Successfully added DataSources, DataSets (Projects>Add Data Source) and we are now getting the
'   LastFetchDate from Jamie's SP, applying it to the URI before calling.
'
'02-19-2014
'
'   Another great day, and it's only 12:23pm so far.
'
'   Creating the DataSource and DataSet for ComerceLD1/NextGen_Profiles was easy, and the QueriesTableAdapter had procESPMergeLibraries
'   with the udt Table Type as an "object".  A little poking around and I simply needed to make a DataTable, with the same name as
'   Jamie's Type, and add the columns as the appropriate data types.  Then, since my Json was already desericalized, I simply For-Each'd
'   through it adding a row for each library with the values found.
'
'   Then, simply called the proc via the queries table adapter, passing the DataTable as the udt object, and bingo!  I almost couldn't
'   believe it, but there were my 2 little libraries, with the appropriate updated dates, happily sitting in the target table.
'
'   Went on to create the .vb to go with the DataSet for NextGen_Profiles in order to get at the return values, etc.
'
'   Then, added in the call to procESPSetLastFetchDate, pulling return values from it as well.
'
'   Finally, added in the logging for each SP call (return values) and logic/logging for exceptions and non-zero return values.  I
'   only call "SetLastFetch" if I do the Merge wihtout error/non=zero result.  
'
'02-20-2014
'   Adding in AppData xml and load, etc.
'   Adding in AlertMail (and Email support)
'
'02-21-2014
'   Expanded the Error Handling and Notifications.
'   Added Retry logic and functionality.  Retry attemps, and Hour to run are part of .config.
'       GetLibrariesHasRun, ...AttemptCount, ...BusyNow, added as public with obvious uses.
'       Retry Interval is based on polling interval which is in .config
'
'   As per Sprint target.  This one is just about ready for prime-time.  Can deploy to a BizTalk Server when in SharedDev.
'
'   Worked on saving json as archives in addition to deserializing, but not pretty.  Confused about System.IO.Stream vs MemoryStream,
'   and basically how to use the intial Stream for both outputs.  I want to save the json and then deserialize to Class, but right
'   now I am deserializing to Class, and then serializing that to json, so it's not the "actual" input json.
'   It's working, but I don't like it.
'
'   ...but enough for this week.
'
'02-24-2014
'   Moving on to Ranking... Started by opening OrdersDataSet in the Designer and Adding Queries, but then decided to just delete the whole thing
'   and Add the DataSource back in, since I was unsure about how to add the complex XML (Ranking) query.  Sure enough, it treated that query as
'   it's own object apart from the QueriesTableAdapter.  Instead, it is its own table and tableadapter.  The only down side to this was that
'   removing the original OrdersDataSet also removed OrdersDataSet.vb (the class added to get at return values and output paramters) but that
'   was easy to recreate with a quick copy/paste since we are now using generic parameter names.
'
'   It would probably work by adding to the OrdersDataSet if I knew how to determine when to add a TableAdapter vs a Query, becuase it appears
'   that I couild have added the ranking request by using Add... TableAdapter instead of Query.
'
'   NOTE: with reference to the above paragraph, when Jamie changed the SP for CartRanking, I removed that single TableAdapter and
'       then recreated it by "Add"ing it, stepping through the same wizard and it worked like a charm, without having to recreate the
'       whole DataSource
'
'   OK, calling to get count and calling for carts without error, but not sure how to digest the xml once I get it, when I get it.
'
'   Need to get the DBTeam and/or Jamie to flag a couple of baskets for ranking, and not reset them so I always get a count and at least 2 victims.
'
'
'02-25-2014
'
'   Having a heckuva time reading and derserializing the SQL XML.
'   1)  When XML spy creates the XSD it makes two of them, one for the root and one for everything else, importing from the schema "1" version.
'   2)  XSD.EXE cannot handle that.
'   3)  Manually took the "1" version and copy pasted the root elements into the XSD.
'   4)  XSD.EXE appears to make a pretty good class from that... but
'   5)  The deserializing always reponds with an XML Document Error (1,2) with the message <RankRequests xmlns=''> was not expected.
'   6)  Tried many things, inclusive of reading from a static file, etc. to no avail.
'   7)  Also, note that XMLSPY takes values passed in the XML file and makes them Enuerations, but by reducing the 
'       ignore enumerating when elements exceed a count of X, I think I supressed that effectively.  That's part of
'       the XMLSpy Create Schema Dialog Box(es).
'
'   The main issue seems to be related to those "ns1:" prefixes and/or the target namepace being a part of the root element in the xsd.
'   Reluctanly give up for today.  Worried.
'
'02-26-2014
'   With an AM colonoscopy behind me (HaHa get it? BEHIND ME!), I hit this fresh again and looks like I may have had a breakthough.
'
'   Based on Iseaching (which yielded exactly zero results on what i am doing, which is injesting SQL XML using a DataSet/TableAdapter/DataTable)
'   There was a comment on XSD.EXE not being able to handle target namespaces, so I took that part of the root attributes out of the schema. (and I think that is all I did)
'
'   Then, when I loaded the RankRequestsSample.xml (which references the above xsd directly) file into XMLSpy it compained about ns1:RankRequest saying it was expecting
'   just RankRequest.  I quicly find/replaced "ns1" with "" and the file saved without issue.
'
'   I then went back to XSD.EXE and recreated the Class.
'
'   Then, hard coded the file read to the altered XML file (with no ns1: prefixes.  I wanted to see if that file would deserialize, fully expecting that
'   even if it did, the "string" deserializer that uses the current SQL XML string to subequently fail.
'
'   Sometimes you get a break.  Not only did it apparrently successfully deserialize the file, but also appears to have deserialized the string (with the ns1: prefixes) as well.
'
'   Looking into this further and now I need to take the classes created and somehow log their contents to verify, but it looks like the steps to make this work
'   are as follows:
'   1) Get some typical XML response that contains multiples of whatever allows for same.
'   2) If XMLSpy creates 2 XSDs [filename].xsd and [filename]1.xsd, most likely the root is in the first and all the repeating nodes are in the latter.
'       If so, copy paste the appropriate schema elements to the "1" version, "PrettyPrint" it and save it as the non "1" version, writing over the previous.
'   3) If it has a "targetnamespace=" element at the root or opening tag area, remove it and save again.
'
'   To test, open up or create the test xml, and temporarily, at least, add a schema reference to the above altered file.  If you get a "green" That should do it.
'
'   If the above is all it takes, that is really not a big deal.  The other option is to look into having the XML output from the SP to do away with any of that
'   namespace stuff, so XMLSpy might create a simple .xsd to beging with.
'
'   This is a little  wierd.... XMLSpy complains about the "ns1:" prefixes when you try to save referencing the altered .xsd, BUT...
'   Either with or without them, file or string input, they deserialize just fine! So, I think the above 3 steps are key, but XMLSpy might not be the final
'   test, you may just have to try the derserializing to see if it works.
'
'   Now on to confirm that my deserializing actually did produce content in the Class Objects!
'
'   ....and therein lies the catch.
'
'   It does not look like the Class Object is populating and/or I can't get to them.
'
'   ...but in the meantime, I solved the "reuse original Json" problem with Get Libraries.
'   I read it into a string with a stream reader, and then deserialize from the string, after archiving it as a .json file.
'
'   That's it for today.  I'll have to get this next dilema solved early tomorrow if I am going to have any chance of meeting my Friday deadline.
'
'02-27-2014
'   Right out of the starting gate, if I deserialize from reading the static XML file with the "ns1:" prefixes removed, it works just fine!
'
'   For some reason, however, even though XMLSpy says the XML with those same prefixes is bad, the deserializer does not return an error, 
'   it just fails to populate the Class Object.
'
'   One unrelated side note:  IF you pass a Null (even if appended to a valid string) to LogIt, nothing is written to the file, No Exception
'   is thrown, but the service just stops functioning.  Easily avoided by testing for IsNothing before doing anything, but very concering that
'   it is such a stealth fatal condition.  The Service is still running, but no longer doing anything!
'
'   ABOUT THE ABOVE PARAGRAPH!  - DON'T REFERENCE ex.InnerException IF IT MIGHT NOT EXIST!!!!!
'
'   Yes, LogIt never even was called because the IsNothing() test itself threw a Null Reference Exception of some sort.  That Exception WAS
'   caught, but there was no ex.InnerException which caused an Exception in the Catch!  That Exception, for some reason, resulted in the
'   application hang, but did not produce an unhandled Exception error anywhere that I can tell.  Certainly not in the EventLog, which is what
'   I would have expected.
'
'   So now, just testing for the Null is it's own Try-Catch which, if thrown, assumes the class is empty.  Now (and sadly, I think I learned
'   this lesson before) the InnerException is only referenced if it tests as NOT IsNothing.
'
'   Jamie has adjusted his XML to not include the "ns1:" prefix and also changed his structure to more closely match the JSON.  After recreating
'   the .xsd and recreating the .vb class using xsd.exe, I can deserialize the return string without issue into the .NET Class.
'
'   Went on to create a .NET class for the JSON cart object, loaded the .Net class from the xml .xsd into the json .NET Class, and finally
'   serialized it into .json, archiving as I went.
'
'   The jsonserializer (a datacontractserializer) puts the elements in alpha order, so posted a question to the ESP (Dan Fish) if that was
'   acceptable.
'
'02-28-2014
'   Dan confirmed that alpha order was ok.
'
'   Set out to adjust my WCF RESTFulDemo to accept a RankRequest and return a response in the expected json form.  Struggled with "Invalid
'   Request" errors until I realized the because of the URI I had coded, the client was not adjusing the encoding and content type properly.
'   
'NOTE: the CLIENT is responsible for chosing the correct encoding and content type.
'   As in, it isn't enough just to pass a json formatted string/stream or xml, 
'   it also must have the appropriate enconding and content type to match.
'
'   OK, I can call my RESTService, I am adjusting the HTTP StatusCodes on the fly to match what will be expected from ESP, I think I am
'   interpreting and catching everything as needed.  I don't think I'll get the json when the Status code is a 400 or anything that
'   actually throws an Web Exception... so I am not totally sure how to determine if it is a badly formatted request, or they failed
'   to find the espLibraryId but the request was formatted OK, because both situations would return a HttpStatusCode=400.
'
'   The only thing I am not doing at this point is calling to update the BasketStatus as a failed/accepted whichever or both that are
'   required by me.  Of course, Alerts, structure cleanup, final logging, archiving, etc., etc. all are due for next week.  I'm good.
'
'   Very Productive 2 weeks so far.
'
'03-03-2014
'   Insterted the call to procESPSetState or similar to Submitted or Failed accordingly.
'
'   Migrated to SD1 (BTDEVBIZ) 
'
'   Added HTTPWebRequest Header key/value per Dan Fish.  Get Libraries succeeded.  Jamie adjusted SP for Rank Carts, and excuted 
'   that process.  Getting 405 HTTP status, but thinking that is expected until ESP completes their processing.  Open question,
'   but the hour is late in the British Isles.
'
'   Installsheild returns a totally unhelpful "Error 1001" when you mis enter the passord (or leave out the domain), and the fact
'   that it still thinks the service name is Service1 is very annoying.
'
'   Had to move the "busynow" flag in Get Count as I was returning BEFORE setting it... duh!
'
'   Service is now turned on long running.  debug logging is on, and it is logging every 2 minutes for the get rank count.
'
'   On or ahead of schedule.  Need to do the User Alerts piece, clean up the architecture, and enable cart/rank retries.  Most
'   error tracking, logging and notifications are in place.
'
'03-04-2014
'   Various issues uncovered and resolved today.  
'
'   1) I Had a typo in the URI for cart ranks, causing the 405 Method Not Allowed.
'   2) I had added the Authentication key/value to Get Libraries, but not Rank Cart, causing a 401 Access Denied or similar.
'   3) They restricted the vendorID to 20 bytes so when we passed a Guid I got a 500 Internal Server Error.
'   4) Their response to a cart rank submission was not formatted per their spec.
'   5) The Submitted Status did not show on the UI.
'   6) They literally needed ":" replaced with "%3A" as a time deliminter in the last fetch date.
'
'   ...but now things seem to be working.
'
'   Hopefully, on to user alerts tomorrow.
'
'03-05-2014
'   BE CAREFUL! Today, I screwed things up a bit when I got confused between OrdersDataSet.vb and RankRequests.vb.
'   OrdersDataSet is NOT generated by xsd.exe, but is the manually created class for getting at the Return Values and Parameters.
'   RankRequests.vb IS generated by xsd.exe and is specific to the one SP inside the OrdersDataSet.
'
'   My confusion resulted in generating OrdersDataSet.vb using xsd.exe after Jamie made a change to procESPGetRankRequests.
'   OrdersDataSet.xsd needed to be rebuilt by removing and adding the procESPGetRankRequests DataTableAdapter, but running XSD.exe, overwrote the manually created Class!
'   Instead, after recreating OrdersDataSet.xsd, what I needed to do was regenerate/edit RankRequests.xsd and then use xsd.exe to re-generate RankRequests.vb!
'   Since Jamie was only adding 1 simple string, I chose to edit the .xsd and then regenerate.  
'
'   I think we are ok again now that I have manually rebuilt the OrdersDataSet.vb.  Now to test.
'
'   I believe I have the UserAlerts piece working, but I have no Carts to rank to see.  I went to get my two, and someone had entered espLibraryIds in them so
'   they went through without issue and I updated the status.  ...at which point there were not more!  So no user alert was generated the first time,
'   and the second time (with a hard coded web exception) there were no carts to go after!
'
'   Still, in good shape for tomrrow.
'
'03-06-2014
'   Happy Birthday, Patricia... wherever you are.
'
'   Slight adjustment to the call to UserAlerts and it is working in LD1, as did my forced Exception.
'
'   Can't forget to add the service.model enpoint/client in the app.config.
'
'   Also poked around a little more to find out how to dump the HTTPWebRequest.Headers name/value pairs.  Only enabled for debugLogging.
'   It only dumps 2 key/value pairs, but one is the espKey.
'
'03-07-2014
'   Slpit out the Rank Carts into its own Class.
'
'   Created several CallBack Events and EventArgs so it could easily call AlertMail, LogIt and FileItUTF8 from the calling Class.  This seems to have worked quite well.
'
'   Enabled retries within Logit which so far is avoiding conflicts in logging.
'
'   Let the RankCart class still do all the notifications and logic, only keeping track of the number of attempts made and whether a retry is in order.
'
'   The callback for RankAttemptComplete does nothing but decide whether or not to retry and/or if retries have been exhausted.
'
'   RankCart does not do it's final updates until all Retries are exhausted.
'
'   Had to add the ESPWinServices and RankRequests Namespaces to some of the Class references, but other than that and creating the Subs/Eventhandlers for the
'   Callbacks, things were pretty much cut/paste.  My EventArgs.vb has all the new EventArgs Public Classes.  I like it.
'
'   With Retries now enabled and functional (pending some actual testing in DEV1) - I have officially hit my mark for Sprint whatever this is.
'
'   All that remains is tightening and bullet-proofing as well as handling whatever comes up in testing.
'
'03-10-2014
'   Added a random generator for integers to provide dynamic, random sleep times to avoid collisions in LogIt.
'
'   Now saving individual carts as xml by serializing each RankRequestsRankRequest.  The XML doesn't exactly match
'   the input XML, but should work for reprocessing.  Probably advisable to have a "Request" and a "Requests" folder so
'   we could do either manually from XML.
'
'   Also added descriptive prefixes to the files in anticipation of doing other things with carts.
'
'   Promoting to dev.  Will create the reprocessing routines a bit later.
'
'   Reviewed and commented the code.  I don't think any architectural changes are needed at this point.  Some of the
'   Subs are a bit convoluted due to having to check for so many different types of Errors, but I don't think Modularizing
'   them any more is worth the effort.
'
'   Note that early on we were reading from static XML for testing, that commented code is still present and would be 
'   a good starting point for the Reprocessing Polling.
'
'03-12-2014
'   MAY HAVE FOUND THE "Service1" problem resolution.  When creating a new Windows Service, TCPIPListenerWinSvc, I noticed that
'   when you "rename" the Service1 Class (and .vb file), it displays a "Sub Main" not found error, and double clicking on that
'   brings up some designer code, and or a prompt to choose the newly renamed module.  Now the designer code, [Project].[Service].Designer.vb, 
'   can be seen by clicking on the "compnents As IContainer" or the Designer child under [Service].vb.  In this case ESPWinServices for both 
'   Project and Service names.
'
'   There, you will see the "Sub Main" as well as an "InitializeComponent()" that has Me.ServiceName="Service1".  That needs to be
'   what you want the Service Name to be and is what InstallSheild seems to use.  Don't edit it in the code.  Bottom Line is this:
'
'   After you rename the Service Class and the .vb file, double click on the "MainSub" error and choose the newly renamed [Project].[Service]
'   as the Target.  Then open the [ServiceName].vb[Design] (right click the [ServiceName].vb and "View Designer", and click anywhere on the background.  
'   Note the ServiceName component in the Properties Window (scroll to the bottom). THAT is where you change Service1 to the Name you want!
'
'   I'm recompiling and testing this right now!
'
'   IT WORKED!  No reinstall needed.  It is dynamic and driven by the above, soley.  I just did my usual replace of the .exe file.  Case Closed!
'
'03-13-2014
'   Discovered today that the HTTP response is passed to the WebException object and available there.  That is how the json (or any content
'   available from GetResponseStream()) is extracted and processed.  Within the Catch of the WebException, you do another GetResponseStream()
'   and go on from there.  We are now getting the json from the ranking attempt when an HTTP 404 is encountered that throws a WebException.
'
'03-14-2014
'   Dan Fish advised looking at the contenttype in the Header to determine whether or not json is available. Adjusted the code to use that as
'   the test, and deserializing the json when we know the format (Ranking Request) or just dumping the json as a string when we aren't sure
'   as in if something goes south with the GetLibraries request, which is supposed to return a list of libraries.
'
'   In all cases, we only try to pull json if that is indicated in the header.contentType
'
'03-19-2014
'   Changed Name of UTD column ESPLibaryID to ESPCollectionHQLibraryID in keeping with the addtion of our own primary key for the 
'   table that will have the name ESPLibraryID.
'
'   Added new DataSource/DataSet for ExceptionLogging.procTS360APILogRequests.
'
'   Created skeleton Sub for RepositoryExceptionLoging.
'
'03-20-2014
'   Somewhere along the line, I got my streams confused and caused myself Cast exceptions when attempting to deserialize json on the
'   cart rank.  To resolve, I went back to doing the CType deserialization and GetResponseStream() all in the same line of code, rather
'   than trying to create a MemoryStream for GetResponseStream, and then deserializing from that.  In Get Libraries, I go from System.IO.Stream
'   to String via StreamReader, to an Encoded MemoryStream via GetBytes on that String, and then deserialize from that.  I do that so I can
'   GetResponseStream once (which is all you CAN do) and then use that stream to both save to a file and then deserialize.  
'
'   That's not needed when I am just attempting to deserialize the json and/or log it as a string. Removing the extra steps/stream(s) resolved
'   the issue.
'
'   Completed the SQL TableAdapter calls and handling for ExceptionLogging.
'
'   Added Event Arguments, Events and CallBacks for ExLogIt (ExceptionLogging to Repository)
'
'   Added all the calls.  Tested with a hard coded call in LD1.
'
'03-27-2014
'   Added concept of working folder and reprocess folder.
'
'   When RankCart is called, the XML cart is written to the working folder.
'   When The Callback to AttemptComplete is made the file is left in the working folder if a retry is going to happen,
'       otherwise, it gets moved into the Processed folder (LogDir).
'
'   If the service is restarted while in the middle of retrying, OnStart will move files from working to the
'       reprocessing folder so the Ranking Attempts will re-commence (from attempt 1).
'
'   The reprocessing folder is polled on the same elapsed timer that goes for the Repository.
'
'03-31-2014
'   March leaving like a Lion, just how it came in.  Deploying to QA1.
'
'04-18-2014
'   Long since passing mustard in QA1, but Get Libraries is repeatedly failing.  Went to look at this and again discovered
'   no entries in the log file, and not even any log file for today.  Restarted, got entries, and Get Libraries logged itself
'   but also suceeded.
'
'   Just in case this is a problem where it tries 10 times and fails, I will send an alert under those conditions, becuase I
'   do get the Alerts that Libraries Failed, just no log entries.  We'll see. Deployed local Dev1, QA1.
'
'04-21-2014
'   Rebuilt setup and deployed to DEV2.
'
'04-29-2014
'   FINALLY figured out the logging issue when GetLibraries Fails.... and it is neither a logging problem, nor is GetLibraries
'   failing!  I never reset the GetLibrariesAttemptCount, so it would run for 5 days, afterwhich the counter is above the threshold,
'   and it never even gets called after that, until the service is restarted.  Duh!
'
'05-12-2014
'   When deploying to prod, forgot to setup the Reprocess folder, and because there was no catch for a folder error, the Rankingbusy flag
'   was never reset after entering the reprocess loop.  No error trapped or logged, but get rank count only executed once or so before the 
'   reprocess attempt disabled it.
'
'   for now, just added in the folder.  will put a try catch in later.
'
'05-15-2014
'   Added the try-catch and deployed.
'
'07-07-2014
'   ESP HQ people noticed they were getting BTKey with leading zeros missing.  I looked and my RankRequests.xsd had BTKey as a Long.
'   The RankRequests that come from the SQL call, when converted to XML have the string correct, but when I break them into singles,
'   that's when they get converted to a long.  I manually edited the .xsd and .vb to change long to string as needed.
'
'   My internal classes were ok, so I'm not quite sure how the xsd/vb got set as a long.
'
'   Deployed and tested to DEV1.  QA and PROD to follow, then I'll see if I can merge the code "up" to 3.0.
'
'09-08-2014
'   Begin Phase II changes for TS360 release 3.1  So far, just created the Classes for the new functionality.
'
'
'01-15-2015
'   The ESP Phase II stuff got pushed out so far, I haven't touched the code since September  Depending on what
'   release it is finally scheduled for, I may have to branch it off to finish Phase II, but since the changes
'   made so far are transparent (just added classes, no functional code) I'll keep this version in 3.1
'
'   Meanwhile, I was getting 4xx statuses from ESP with no JSON status being logged, even through I thought I
'   was reading/logging it.  Looking at the code, it doesn't appear that I ever coded the "LogIt" part... so
'   added that, and also added a log entry if the content type does not appear to be JSON.
'
'03-06-2015
'   Checked in old code, and Branched to Release 3.2 from 3.1 to begin final coding.
'
'03-10-2015
'   My first attempt at updating the Data Sources did not work very well, and I ended up abandoning all my changes and
'   starting over again.  I think, however, that I have updated the dataset (OrdersDataSet was the only one that changed)
'   ...and I think this might be the proper way going forward:
'
'   1) If the DB connection strings have changed (in this case from LD1 to LD2) the first thing to do is to update the
'       connection strings in the settings/config and save them.
'
'   2) Then, view the DataSources window, right click on the target DataSet, and select "Configure Data Source with Wizard"
'
'   3) That will bring up the tree of objects, but first hit the back button, and expand the connection string to verify
'       that the wizard is pointing to the new connection string.
'
'   4) Then, when you move forward again and expand the objects (in this case, stored procedures) they will contain the
'       new and/or updated objects, with the previously existing/referenced items already checked.
'
'   5) Check the new items that you want to add, and click Finish.
'
'   Note that doing it this way rather than deleting and adding back in preserves any custom code behind modules (OrdersDataSet.vb
'   in this case) so that it doesn't have to be saved/recreated.  Also, note that the way to Delete a Data Source, is not from
'   the tool window, but by deleting the .xsd in the Solution Explorer, which also blows away the associated code-behind files.
'
'   I think we are OK, but oddly, the 2 new "get count" procs show in their own table adapters, whereas the original get count fo
'   the ranking requests is included in the queries table adapter.  Not sure why or if that is an issue.  
'
'   Note also, that since these are stubs, they all return "Column1" (except rank requests).  I also don't see the reference
'   to the usertable that is supposed to be passed to procESPSetStatusQueue... but maybe that is also due to the skeletal nature
'   of all these, but more likely I need to create the Data Table object as I did for updating Libraries each day, because
'   I do see the utbl reference in the actual SP parameters. See 2-19-2014.
'
'03-11-2015
'   replaced "rankSubmitted" and "rankFailed" with "Submitted" and "Failed" for ESP II changes.  I don't think the DB group has
'   a firm handle on either the input for updating the ESPState table, nor the output for Distribution and FundMonitoring.  It
'   does appear, however, that the ESP site might have its functionality in place.  I got an empty Jobs array, and successfully
'   submitted a cart ranking, in addition to picking up Libraries today.
'
'03-12-2015
'   Mocked up .xsd and xsd.exe'd to .vb for DistributionRequests and FundMonitoring Requests, so I had the classes to work
'   with.  Revisited the Json .vb classes to match to what is expected for going to/from ESP.  Created new DistCart.vb and 
'   FundCart.vb classes, and did a hefty amount of cloning to enable the new transactions.
'
'   In some places, now passing a type of "RANK", "DIST" or "FUND" and adjusting the code via Selects or If-ElseIf statements.
'
'   Still no new samples from ESP, and SPs are not finished, but moving ahead anyway.
'
'   Note that the "count" SPs for the requests now reside in the Queries Table Adapter as expected, but had to remove them
'   and add them back in (using the DataSource Wizard) in order to affect the change.  Not seeing the new Enums in the User
'   Alerts Template, so will research that further when Jamie gives me the LD2 URL.
'
'   Alot of the heavy lifting is done, especially if we get the guts of FundCart reworked today.  Working a little late
'   to go to the MeetUp Acoustic group at Mannion's in Somerville, so I may have time to do the Fund pieces.
'
'03-16-2015
'   Refreshed the UserAlertService Service Reference by removing and adding back in with the LD2 URL.  Went through and
'   updated all the UserAlert calls.  Since each type has its own class and code, each has it's own "SendUserAlert" function
'   so they were easy to adapt, and all the Template ENums did show up after the refresh.
'
'   Went on to add the additional cart items for DIST request.  I have no logic that makes use of the fundMonitoring Flag,
'   which I am still calling AutoFundMonitoring.  Awaiting the current ESP spec to finalize all the json names and structure.
'
'   Also, Bruce has been turned loose for the SP's that "Marvin" needs.  My XSD's and Classes will change with those 
'   changes as well, I am sure.
'
'   Pending the finalized schemas/classes and the logic for AutoFundMonitoring, We are about ready to do end to end with
'   UserAlerts, but without updating the ESPState Table.  Bruce has that SP as well.
'
'03-19-2015
'   Hit this again hard after being sidetracked by IrisBisacMasher.
'
'   Getting/creating sample XML for the three transactions proved to be much more time consuming than I thought.  Issues
'   with xmlns, Having the namespace on the tags under the root or not, in conjunction with what was in the shcema/class
'   proved to be crtical in actually populating the class from the XML.  Not really sure what I will get when I actually
'   do get carts from the Repository.
'
'   Then ESP was returning "401" forbidden http errors for every call.  Ran out of time for the day.  Will move to DEV2
'   tomorrow and continue.
'
'03-20-2015
'   Looks like I still have today before expecting to try round trip testing on Monday.  Went in and added Fund/Dist/Rank
'   qualifiers to most of the messages, logging, alerts, and error reporting.  Will compile and deploy to DEV2 now.
'
'03-25-2015
'   Wednesday and we just might have functionality.  However, this code is in sore need of modularizing.  It's some of
'   my worst work yet.  This has been making it awfully hard to troubleshoot.
'
'   A couple of notes.  It doesn't matter what the names are of the columns in the DataTable that are to be passed
'   as a userdefined table.  Only the order and number of columns (and data types) are important.  If you don't populate
'   a column, the fields get shifted and populate into the wrong columns in the DB.
'
'   To that end, in order to pass a Null for column content (so the existing value won't be overwritten) you must use
'   DBNull.value as the content.
'
'   Also note that almost always "...will be truncated" means something is longer than waht the SQL DataType allows.
'
'   And, in the case of some arrays, even after using a deserializer to create the object, you may still need to
'   test for their existance before doing a for-each on them.  The Array itself may not have been created.  This is
'   true for the fundcodes array that comes back in a Distribution Response that does not have fund monitoring.
'
'   I still need to put something in for "paging" in case the queue items get above 100, clean up all my debug code,
'   and begin to look at modularizing this monster.
'
'03-30-2015
'   A couple of minor changes for FUND only requests, now that they all are functional.  Still needed adjustments
'   to the length of fields in the udt.  Some mis matches between json Serializer objects, and fund/dist/rank word
'   cleanups.  Code still messy, but functional.
'
'   Deploy to QA1 tonight.
'
'04-01-2015
'   Started DoACart.vb, to combine [Rank-Dist-Fund]Cart.vb into a single module.  I think I will have to use Classes
'   to represent the Function specific classes/objects, and then somehow use ENums to reference them so I can address
'   the proper types without having Select Case Type statements all over the place.  I don't know if that will work.
'
'04-02-2015
'   Recompiling with Install for deployment to PE environment.
'
'   Puzzled over the fact that the service.model endpoint is not included in the generated App.config file, but noticed a
'   reference to that fact from over a year ago telling myself to not to forget to add it in!  I couldn't find any other
'   reference that mentioned why it isn't automatically there, why it is needed, and how I discovered that it was both
'   omitted and required in order to make the UserAlerts service endpoint configurable.
'
'04-03-2015 - 04-08-2015
'   Decided to consolidate the XXXXCart.vb modules to one DoACart.vb to take all functions, as the first step in
'   cleaning up and modularizing the code.  It has turned out to be quite the bear.  Passing "Objects" and converting
'   them after the fact to different types has proved to be challenging, if not with limitations.
'   
'   It looks like I will be able to pass a single cart object, but I have also created a "dummy" CartObject that
'   holds some sample typed objects when the ChangeType can be affected with Object.GetType.  I also found that
'   the CType that I was using for reading streams to json will also work with Convert.ChangeType, which makes
'   that easily dynamic.
'
'   I have, however, had to result to a few instances of Select-Case to create some function specific objects.  I've
'   tried to put as much of that at the top of the module as possible, but a couple of places after the response comes
'   back requires selects as well, due mostly, if not completely, to the fact that Distribution responses come back
'   with multiple statuses (or can anyway), so sometimes there is an array, other times not.
'
'04-08-2015
'   Finally got the DIST round trip (with expected errors), so now will wire up the fund and ranks and see what happens.
'
'04-09-2015
'   Some more clean-up and now getting all three functions to go round trip, including the proper handling of the json
'   and objects during a web exception.  "Commonized" the Json Response types, for the most part, which makes them
'   easier to handle (avoiding Object Instance Exceptions) even though some elements are not used or populated
'   in some responses.
'
'04-14-2015
'   Work on getting all variations working smoothly continued through Friday, 4-10.  PTO Monday, 4-13. Today, started
'   to at least comment similar sections of code in hopes of going back and breaking them out into separate subs/functions.
'   Also, added a "success" user altert for FUND only transactions, since the cart doesn't "come back" via the API.  This
'   worked well with enabling With/Without Fund Monitoring on DISTs, and re-enabling Library vs System on Ranks.
'
'   Also discovered a code errors on the "retry" submissions for DIST and FUND which may have accounted for the odd 
'   "attempts" values and double notify alerts.  For some reason the "attempts" value was going back as "retrycount + retrydelay"
'   hence the 301 value of 300 seconds plus 1!... and with a delay of 0 which should have been 300.  On top of that one of
'   the exception handlers said "If LastRetry", but the other said "If Not LastRetry" with the same logic underneath.  They
'   both should have been "If Not LastRetry"
'
'   Next is more modularizing/commonizing for Alerts, SPCalls, Checking SP Return Values, and maybe extracting Json, all of
'   which are called repeatedly within the pasta.
'
'04-15-2015
'   Adjusted SendUserAlert to allow for getting and using the Carts Page URL to send back with the appended CartID for
'   the "FUND Success" Alert.
'
'04-16-2015
'   Discovered late yesterday that WebEx Fail User Alerts were not working.  The code that set the default Enum for the template
'   had been removed/omitted, so a Null Reference was created.  Also, I never added the code to populate the invalid Branch/Fund
'   into the udts for same.  The one snippet for this was not close to the code for Updating the ESPStateTable, so I moved it
'   closer and added it to the various failure scenarios.  Deployed to DEV2, QA1, PEWFE2 and locally.
'

Imports System.Net
Imports System.Runtime.Serialization.Json
Imports System.IO
Imports System.Text
Imports System.Net.Mail
Imports System.Data.SqlClient
Imports System.Threading



Public Class ESPWinServices

    Public Polling As Boolean = False
    Public MachineName As String = My.Computer.Name

    Public GetLibrariesHasRun As Boolean = False
    Public GetLibrariesBusyNow As Boolean = False
    Public GetJobsBusyNow As Boolean = False
    Public GetLibrariesAttemptCount As Short = 0
    Public GetJobsAttemptCount As Short = 0
    Public DoRankingsBusyNow As Boolean = False
    Public DoDistribsBusyNow As Boolean = False
    Public DoFundMonsBusyNow As Boolean = False

    Public MyAppDomain As System.AppDomain
    Public MyAppData As AppData = Nothing

    Dim WithEvents GetLibPollTimer As New Timers.Timer
    Dim WithEvents CartJobsPollTimer As New Timers.Timer

    Dim ESPCartPickupOverride As Boolean = False

    Public Class RankRequestJson

        Public espLibraryId As String
        Public userName As String
        Public cartId As String
        Public items As List(Of item)

    End Class


    Public Class RankResponseJson

        'Public statusCode As Integer
        Public statusCode As String
        Public statusMessage As String
        Public jobId As String
        Public branches As List(Of distBranch)  'Never used, but included to have in common with other responses
        Public fundCodes As List(Of distFund)   'Never used, but included to have in common with other responses

    End Class

    'just a URL request, so no request/response
    Public Class JobsListJson

        Public jobs As List(Of JobJson)

    End Class
    Public Class JobJson

        Public jobId As String
        Public jobType As String
        Public cartId As String
        Public submittedAt As String
        Public processedAt As String
        Public returnedAt As String
        Public status As String

    End Class

    Public Class DistributeRequestJson

        Public espLibraryId As String
        Public userName As String
        Public fundMonitoring As String
        Public cartId As String
        Public branches As List(Of branch)
        Public items As List(Of Item)


    End Class

    Public Class branch

        Public branchId As String
        Public code As String

    End Class

    Public Class Item

        Public lineItemId As String
        Public vendorId As String
        Public fundId As String
        Public fundCode As String
        Public price As Decimal
        Public quantity As Integer
        Public author As String
        Public series As String
        Public bisac As String
        Public dewey As String
        Public publisher As String

    End Class

    Public Class DistributeResponseJson

        Public statusCodes As List(Of statusCode)
        Public jobId As String
        Public branches As List(Of distBranch)
        Public fundCodes As List(Of distFund)

    End Class

    Public Class statusCode

        Public code As String
        Public message As String

    End Class

    Public Class distBranch

        Public branchId As String
        Public code As String
        Public status As String

    End Class

    Public Class distFund

        Public fundId As String
        Public code As String
        Public status As String

    End Class

    Public Class acceptFundRequestJson

        Public espLibraryId As String
        Public userName As String
        Public cartId As String
        Public items As List(Of item)

    End Class


    Public Class acceptFundResponseJson

        Public statusCode As String
        Public statusMessage As String
        Public branches As List(Of distBranch)  'Never used, but included to have in common with other responses
        Public fundCodes As List(Of distFund) 'duplicate, so we use distFund instead of creating acceptFund

    End Class

    Public Class CartFunctions
        Public RequestType As Object
        Public ResponseType As Object
        Public Type As CartType
        Public RequestSerializer As DataContractJsonSerializer
        Public ResponseSerializer As DataContractJsonSerializer
        Public EventArguments As EventArgs
        Public Items As IEnumerable(Of Object)
        Public Branches As IEnumerable(Of Object)
        Public FundCodes As IEnumerable(Of Object)
        Public jsonRequest As Object
        Public jsonResponse As Object
        Public UserAlertTemplateEnum As UserAlertsService.AlertMessageTemplateIDEnum
        Public UserAlertTemplateEnum2 As UserAlertsService.AlertMessageTemplateIDEnum
        Public URIPostFix As String



    End Class

    Public Enum CartType
        DIST
        FUND
        RANK
    End Enum


    Public CartObject As List(Of CartFunctions)

    'UserDefinedTables going to SQL.  Columns are added in OnStart
    Public udtblLibraries As New DataTable("utblESPLibraryUpdateType")
    Public udtblESPStatus As New DataTable("utblESPStatusType")
    Public udtblBranches As New DataTable("utblESPInvalidCodesType")
    Public udtblFundCodes As New DataTable("utblESPInvalicCodesType")

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        MyAppDomain = System.AppDomain.CurrentDomain


        If Not System.Diagnostics.EventLog.SourceExists("ESPWinServices") Then
            System.Diagnostics.EventLog.CreateEventSource("ESPWinServices", "Application")
        End If

        EventLog1.Source = "ESPWinServices"
        EventLog1.Log = "Application"

    End Sub


    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.


        Try
            EventLog1.WriteEntry("ESP Interfaces Windows Service is Starting as " & MyAppDomain.FriendlyName & " in Startup Path = " & MyAppDomain.BaseDirectory)


            'Configure the Processing Timer and start it up.
            GetLibPollTimer.Interval = My.Settings.LibraryPollingIntervalSeconds * 1000
            GetLibPollTimer.Enabled = My.Settings.LibraryEnablePolling
            GetLibPollTimer.AutoReset = True

            CartJobsPollTimer.Interval = My.Settings.CartJobsIntervalSeconds * 1000
            CartJobsPollTimer.Enabled = My.Settings.ESPCartsEnabled
            CartJobsPollTimer.AutoReset = True

            LoadAppData()

            'This tests the SQL connection on startup, and alerts if it cannot be opened as configured, but this is not a fatal error.
            Dim sqlConnection1 As New SqlConnection(My.Settings.OrdersConnectionString)
            Dim sqlConnection2 As New SqlConnection(My.Settings.NextGen_ProfilesConnectionString)

            Try
                'We think this is tried when the app starts behind the scenes, but we trap for it here just in case
                sqlConnection1.Open()
                sqlConnection1.Close()

            Catch sex1 As Exception

                LogIt("AHHHH! Couldn't get through OnStart without SQL Connect error: " & sex1.Message & " to: " & My.Settings.OrdersConnectionString)
                AlertMail(MyAppDomain.FriendlyName & " Started with SQL Connect Errors.", Me.ToString & " is starting but could not complete OnStart without SQL Exception(s). ", "beep")

            Finally

                If Not IsNothing(sqlConnection1) Then
                    sqlConnection1.Close()
                    sqlConnection1 = Nothing

                End If

            End Try

            Try

                sqlConnection2.Open()
                sqlConnection2.Close()

            Catch sex2 As Exception
                LogIt("AHHHH! Couldn't get through OnStart without SQL Connect error: " & sex2.Message & " to: " & My.Settings.NextGen_ProfilesConnectionString)
                AlertMail(MyAppDomain.FriendlyName & " Started with SQL Connect Errors.", Me.ToString & " is starting but could not complete OnStart without SQL Exception(s). ", "beep")

            Finally

                If Not IsNothing(sqlConnection2) Then
                    sqlConnection2.Close()
                    sqlConnection2 = Nothing

                End If
            End Try

            Try

                'Now Create columns for DataTables that will be passed to SQL ===========================================================================

                udtblLibraries.Columns.Add("ESPCollectionHQLibraryID", GetType(String))
                udtblLibraries.Columns.Add("ESPLibraryName", GetType(String))
                udtblLibraries.Columns.Add("ESPEnabled", GetType(Boolean))

                udtblFundCodes.Columns.Add("BasketSummaryID", GetType(String))
                udtblFundCodes.Columns.Add("GridCodeID", GetType(String))

                udtblBranches.Columns.Add("BasketSummaryID", GetType(String))
                udtblBranches.Columns.Add("GridCodeID", GetType(String))

                udtblESPStatus.Columns.Add("BasketSummaryID", GetType(String))
                udtblESPStatus.Columns.Add("ESPJobType", GetType(String))
                udtblESPStatus.Columns.Add("ESPStateType", GetType(String))
                udtblESPStatus.Columns.Add("ESPJobID", GetType(String))
                udtblESPStatus.Columns.Add("ESPCartStatus", GetType(String))
                udtblESPStatus.Columns.Add("ErrorMessage", GetType(String))
                udtblESPStatus.Columns.Add("ESPResponseJSON", GetType(String))


            Catch sex3 As Exception
                Dim errmess As String = "AHHHH! Couldn't get through OnStart without an exception adding udtbl colums: " & sex3.Message
                LogIt(errmess)
                AlertMail(MyAppDomain.FriendlyName & " Started with table column errors. ", Me.ToString & " OnStart Exception(s). " & errmess, "beep")

            End Try

            LogIt("ESP Interface Windows Service is Starting as " & MyAppDomain.FriendlyName & " in Startup Path = " & MyAppDomain.BaseDirectory)
            AlertMail(Me.ToString & " Starting.", Me.ToString & " is starting and has loaded AppData. ", "mail")

            CartObject = New List(Of CartFunctions)

            Dim where As Short = 0
            ReInitializeWorkingFiles()
            Try
                where = 1
                CartObject.Add(New CartFunctions)
                CartObject.Add(New CartFunctions)
                CartObject.Add(New CartFunctions)
                where = 2
                CartObject(0).Type = CartType.DIST
                CartObject(1).Type = CartType.FUND
                CartObject(2).Type = CartType.RANK
                where = 3
                'DistCart
                With CartObject(0)
                    .RequestType = New DistributionRequestsDistributionRequest
                    .EventArguments = New CartAttemptCompleteEventArgs
                    .RequestSerializer = New DataContractJsonSerializer(GetType(DistributeRequestJson))
                    .ResponseSerializer = New DataContractJsonSerializer(GetType(DistributeResponseJson))
                    .Items = New List(Of Item)
                    .Branches = New List(Of distBranch)
                    .FundCodes = New List(Of distFund)
                    .jsonRequest = New ESPWinServices.DistributeRequestJson
                    .jsonResponse = New ESPWinServices.DistributeResponseJson
                    .UserAlertTemplateEnum = UserAlertsService.AlertMessageTemplateIDEnum.ESPDistWOFundFail
                    .UserAlertTemplateEnum2 = UserAlertsService.AlertMessageTemplateIDEnum.ESPDistWFundFail
                    .URIPostFix = "jobs/distribute"

                End With
                where = 4
                'FundCart
                With CartObject(1)
                    .RequestType = New FundRequestsFundRequest
                    .EventArguments = New FundAttemptCompleteEventArgs
                    .RequestSerializer = New DataContractJsonSerializer(GetType(acceptFundRequestJson))
                    .ResponseSerializer = New DataContractJsonSerializer(GetType(acceptFundResponseJson))
                    .Items = New List(Of Item)
                    '.Branches = New List(Of distBranch)
                    '.FundCodes = New List(Of distFund)
                    .jsonRequest = New ESPWinServices.acceptFundRequestJson
                    .jsonResponse = New ESPWinServices.acceptFundResponseJson
                    .UserAlertTemplateEnum = UserAlertsService.AlertMessageTemplateIDEnum.ESPFundFail
                    .UserAlertTemplateEnum2 = UserAlertsService.AlertMessageTemplateIDEnum.ESPFundComplete
                    .URIPostFix = "carts/"

                End With
                where = 5
                'RankCart
                With CartObject(2)
                    .RequestType = New RankRequestsRankRequest
                    .EventArguments = New RankAttemptCompleteEventArgs
                    .RequestSerializer = New DataContractJsonSerializer(GetType(RankRequestJson))
                    .ResponseSerializer = New DataContractJsonSerializer(GetType(RankResponseJson))
                    .Items = New List(Of Item)
                    '.Branches = New List(Of distBranch)
                    '.FundCodes = New List(Of distFund)
                    .jsonRequest = New ESPWinServices.RankRequestJson
                    .jsonResponse = New ESPWinServices.RankResponseJson
                    .UserAlertTemplateEnum = UserAlertsService.AlertMessageTemplateIDEnum.ESPRankFailSystem
                    .UserAlertTemplateEnum2 = UserAlertsService.AlertMessageTemplateIDEnum.ESPRankFailLibrary
                    .URIPostFix = "jobs/rank"
                End With

            Catch ex As Exception

                LogIt(where.ToString & ")-AHHHH! Couldn't get through OnStart creating CartObjects without Exception " & ex.Message)
                AlertMail(MyAppDomain.FriendlyName & " Started with Errors.", Me.ToString & " is starting but could not create CartObjects without Exception(s). ", "beep")

            End Try

        Catch ex As Exception
            LogIt("AHHHH! Couldn't get through OnStart without Exception " & ex.Message)
            AlertMail(MyAppDomain.FriendlyName & " Started with Errors.", Me.ToString & " is starting but could not complete OnStart without Exception(s). ", "beep")
            'Right now, this is the only place we programatically stop the service, because if we can't load AppData, we're hosed anyway.
            [Stop]()
        End Try


    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        EventLog1.WriteEntry(Me.ToString & " Windows Service is Stopping.")

        AlertMail(Me.ToString & " Closing.", Me.ToString & " is closing.", "beep")

        LogIt(Me.ToString & " is closing.")


    End Sub

    Sub ReInitializeWorkingFiles()
        'This would take any "stuck" files in the working folder and move them back to the inbound folder.
        '   With only one inbound folder, this is a simple move of everthing it finds.  If we add new inbound folders that
        '   perform individualized processes, additional coding will be required to target the proper inbound folder(s) on a case by case basis.


        'Dim WorkFilePath As String = "" 'FullPath
        Dim WorkFileName As String = "" 'Name only
        Dim WorkFileInfo As FileInfo = Nothing


        For Each WorkFile As String In My.Computer.FileSystem.GetFiles(My.Settings.WorkingDir, FileIO.SearchOption.SearchTopLevelOnly, "*.xml")
            'Here (or in the Try) is where you would determine if this file belongs to this PollFolder and Continue For if not...
            '   but with only one polling folder and no coded mechanism to make the decision, we just move 'em all.
            Try
                WorkFileInfo = New FileInfo(WorkFile)

                If InStr(UCase(WorkFile), "RANK") > 0 Then
                    My.Computer.FileSystem.MoveFile(WorkFile, My.Settings.ReprocessRankDir & "\" & WorkFileInfo.Name)
                    LogIt(WorkFile & " moved to " & My.Settings.ReprocessRankDir & " on startup.")

                ElseIf InStr(UCase(WorkFile), "DIST") > 0 Then
                    My.Computer.FileSystem.MoveFile(WorkFile, My.Settings.ReprocessDistDir & "\" & WorkFileInfo.Name)
                    LogIt(WorkFile & " moved to " & My.Settings.ReprocessDistDir & " on startup.")

                ElseIf InStr(UCase(WorkFile), "FUND") > 0 Then
                    My.Computer.FileSystem.MoveFile(WorkFile, My.Settings.ReprocessFundDir & "\" & WorkFileInfo.Name)
                    LogIt(WorkFile & " moved to " & My.Settings.ReprocessFundDir & " on startup.")

                Else
                    LogIt(WorkFile & " type could not be identified and was not moved on startup.")

                End If

            Catch ex As Exception

                LogIt("Could not move " & WorkFile & " without exception: " & ex.Message)

            End Try

        Next




    End Sub


    Sub GetLibraries()

        If GetLibrariesBusyNow Then
            LogIt("GetLibraries() is called but GetLibrariesBusyNow")
            Exit Sub
        End If

        GetLibrariesBusyNow = True
        GetLibrariesAttemptCount += 1

        Dim content As String = ""
        Dim Method As String = "GET"

        Dim retvalue As Int32 = 0
        Dim errMess As String = ""
        Dim ftchDate As Date = Now

        Dim tbladaptr As New OrdersDataSetTableAdapters.QueriesTableAdapter

        'Get the Last Fetch Date ===================================================
        Try
            tbladaptr.procESPGetLastLibraryFetchDate(Now, "OK")
            'Now we need to read what the SP returned to us


            'Note that the below values are made accessible by creating a Partial Class related to the DataSet.  
            '   see AppWithSQLTests for details.
            retvalue = tbladaptr.GetReturnValue(0)
            errMess = tbladaptr.GetParam2(0)
            ftchDate = tbladaptr.GetParam1(0)


            DLogIt("Results of GetLastFetchDate :" & " Message=" & errMess & " ReturnValue=" & retvalue.ToString & " LastFetched: " & ftchDate.ToString)

            If Not retvalue = 0 Then
                'Send an Alert that the SP returned an error.
                Dim AlertMess As String = "Error Calling SP procESPGetLastLibraryFetchDate." & " Return value=" & retvalue.ToString & " Message=" & errMess
                AlertMail("GetLastUpdate Failed.", AlertMess, "dbESPInfo")
                LogIt("Error. Got Non Zero returnvalue. " & AlertMess)
                ExLogIt("procESPGetLastLibraryFetchDate", AlertMess)
                GetLibrariesBusyNow = False
                Exit Sub
            End If

        Catch ex As Exception
            LogIt("Exception Encountered Fetching Date: " & ex.Message)
            AlertMail("GetLastUpdate Failed.", "Exception Encounterd Getting Last Fetch Date " & ex.Message, "dbESPInfo")
            ExLogIt("procESPGetLastLibraryFetchDate", ex.Message)
            GetLibrariesBusyNow = False
            Exit Sub
        End Try

        DLogIt("Last Fetch Date Returned from SQL: " & ftchDate.ToString)

        'in case it's empty and ends up with the 01-01-1984 or whatever that value is, set it to 2 days ago so we are sure to get yesterday, all of it.
        If DateDiff(DateInterval.Year, ftchDate, Now) > 1 Then ftchDate = DateAdd(DateInterval.Day, -2, Now)

        'in case, when empty, it comes back as today's date (which I believe is how it's coded) set it to 2 days ago so that is always the "default" date.
        If DateDiff(DateInterval.Day, ftchDate, Now) < 1 Then ftchDate = DateAdd(DateInterval.Day, -2, Now)


        'Format the Date so it can be passed the way ESP needs it in the Get ==================================================
        Dim strFD As String = Strings.Format(ftchDate, "yyyy-MM-ddTHH") & "%3A" & Strings.Format(ftchDate, "mm") & "%3A" & Strings.Format(ftchDate, "ss.fffZ")

        Dim uri As String = My.Settings.ESPBaseURI & "library?lastFetchDate=" & strFD

        'for testing, gimme all of them
        'Dim uri As String = My.Settings.ESPBaseURI & "library"

        DLogIt("Calling: " & uri)

        Dim req As HttpWebRequest = TryCast(WebRequest.Create(uri), HttpWebRequest)
        req.KeepAlive = False
        req.Method = Method.ToUpper()

        req.Headers.Add("espKey", My.Settings.VendorKey)
        req.Headers.Add("api-version", My.Settings.APIVersion)

        Dim resp As HttpWebResponse
        Dim jsonSerializer As DataContractJsonSerializer
        Dim Libraries As LibraryData

        'Make the Get to ESP to Fetch any Libraries updated since the last Fetch==============================================
        Try

            Try
                resp = TryCast(req.GetResponse(), HttpWebResponse)

            Catch ex As Exception
                LogIt("Exception During HttpWebResponse: " & ex.Message.ToString())
                AlertMail("ESP Get Libraries Failed.", "Exception Encounterd Creating HttpWebResponse " & ex.Message, "restESPInfo")
                ExLogIt(uri, "Exception Casting HttpWebResponse: " & ex.Message)
                GetLibrariesBusyNow = False
                Exit Sub

            End Try

            'This IF is really for debugging.  If we want to dump the response we would change LIBRARY below to something else
            If (uri.ToUpper.Contains("LIBRARY")) Then

                Dim jsonSerializedString As String = ""
                'Deserialize the json returned into the corresponding Class of LibraryData =========================================================================
                Try
                    'created a serializer/deserializer for LibraryData
                    jsonSerializer = New DataContractJsonSerializer(GetType(LibraryData))

                    'create a stream for getting the RESThttp response
                    Dim iostrLibHttpResp As System.IO.Stream

                    'load the response into the stream
                    iostrLibHttpResp = resp.GetResponseStream()

                    'now a reader to read the resulting stream into a String
                    Dim sreader As New StreamReader(iostrLibHttpResp)

                    '... and create the json string
                    jsonSerializedString = sreader.ReadToEnd

                    'then archive it for what it's worth.
                    FileItUTF8(My.Settings.LogDir, "json", "Libraries", jsonSerializedString)

                    'now for the trick of saving the original json string and then using the same string it to deserialize
                    '   NOTE the use of Encoding.Unicode.GetBytes to "fill" the stream with the string
                    Dim memstrJsonString As New MemoryStream(Encoding.Unicode.GetBytes(jsonSerializedString))

                    'Deserialize the json into Libraries collection
                    Libraries = CType(jsonSerializer.ReadObject(memstrJsonString), LibraryData)

                    'and don't forget to clean up after yourself.
                    sreader.Close()
                    sreader.Dispose()

                    memstrJsonString.Close()
                    memstrJsonString.Dispose()

                    iostrLibHttpResp.Close()
                    iostrLibHttpResp.Dispose()

                    jsonSerializer = Nothing


                Catch wex As WebException

                    LogIt("WebException During Libraries GetResponseStream : " & wex.Message.ToString())

                    Dim exResponse As WebResponse = wex.Response
                    Dim exStatusCode As HttpStatusCode

                    Dim httpResponse As HttpWebResponse = CType(exResponse, HttpWebResponse)
                    exStatusCode = httpResponse.StatusCode

                    Dim mshttp As MemoryStream = Nothing
                    Dim msr As StreamReader = Nothing


                    'Note that the Library Response is a list of libraries,
                    '   so if there is a problem, I'm not sure what the json will be, if there is any,
                    '   so for now we'll just read the response as a string and log it.

                    If InStr(httpResponse.ContentType.ToUpper, "JSON") > 0 Then

                        Try

                            'We're not deserializing because we don't really know what json we might get in an exception.
                            'jsonSerializer = New DataContractJsonSerializer(GetType(ESPWinServices.RankResponseJson))

                            'We'll get the response stream into a memory stream for converting to string rather than deserializing.
                            mshttp = exResponse.GetResponseStream()
                            mshttp.Position = 0

                            'now a reader to read the resulting stream into a String
                            msr = New StreamReader(mshttp)

                            '... and create the json string
                            jsonSerializedString = msr.ReadToEnd

                            LogIt("Json content returned: " & jsonSerializedString)


                        Catch ex As Exception


                            LogIt("Exception Encountered trying to get json from HTTP response: " & ex.Message)

                        Finally


                            mshttp.Close()
                            mshttp = Nothing
                            msr.Close()
                            msr = Nothing

                        End Try

                    End If

                    AlertMail("ESP Get Libraries Failed.", "WebException Encounterd Getting Response Stream " & wex.Message, "restESPInfo")
                    ExLogIt(uri, "WebException Encounterd Getting Response Stream ", "", jsonSerializedString)
                    GetLibrariesBusyNow = False
                    Exit Sub


                Catch ex As Exception
                    LogIt("Exception During GetResponseStream : " & ex.Message.ToString())
                    AlertMail("ESP Get Libraries Failed.", "Exception Encounterd Getting Response Stream " & ex.Message, "restESPInfo")
                    ExLogIt(uri, "WebException Encounterd Getting Response Stream ", "", jsonSerializedString)
                    GetLibrariesBusyNow = False
                    Exit Sub

                End Try

                'the below is now done at startup, with the other userDefinedTables
                'Now Create and populate the DataTable that will be passed to SQL ===========================================================================
                'Dim udtblLibraries As New DataTable("utblESPLibraryUpdateType")
                'udtblLibraries.Columns.Add("ESPCollectionHQLibraryID", GetType(String))
                'udtblLibraries.Columns.Add("ESPLibraryName", GetType(String))
                'udtblLibraries.Columns.Add("ESPEnabled", GetType(Boolean))

                udtblLibraries.Clear()


                For Each Library As Library In Libraries.libraries
                    'udtable.Rows.Add(Library.espLibraryId, Library.espLibraryName, True) 'to force a change, since one of the canned ones comes back as false.
                    udtblLibraries.Rows.Add(Library.espLibraryId, Library.espLibraryName, Library.enabled)
                Next

                If My.Settings.DebugLogging = True Then
                    For Each Library As Library In Libraries.libraries
                        LogIt("Name=" & Library.espLibraryName & vbCrLf & "ID=" & Library.espLibraryId & vbCrLf & "Enabled=" & Library.enabled.ToString & vbCrLf)
                    Next
                End If

                Dim ptableadapter As New NextGen_ProfilesDataSetTableAdapters.QueriesTableAdapter

                'With the DataTable popuated, make the call to SQL and pass it ================================================================================
                Try
                    ptableadapter.procESPMergeLibraries(udtblLibraries, "OK")

                    retvalue = ptableadapter.GetReturnValue(0)
                    errMess = ptableadapter.GetParam2(0)
                    LogIt("Results of procESPMergeLibraries: " & "ReturnValue=" & retvalue.ToString & " Message=" & errMess)

                    If retvalue = 0 Then
                        Try
                            tbladaptr.procESPSetLastLibraryFetchDate("OK")
                            retvalue = tbladaptr.GetReturnValue(2)
                            errMess = tbladaptr.GetParam1(2)

                            If Not retvalue = 0 Then

                                LogIt("procESPSetLastLibraryFetchDate returned Non Zero.")
                                AlertMail("ESP SetLastUpdate Returned Non-Zero.", "Error Encounterd Getting Last Fetch Date " & "ReturnValue=" & retvalue.ToString & " Message=" & errMess, "dbESPInfo")
                                ExLogIt("procESPSetLastLibraryFetchDate", "Error Encounterd Getting Last Fetch Date " & "ReturnValue=" & retvalue.ToString & " Message=" & errMess)
                                GetLibrariesBusyNow = False
                                Exit Sub

                            End If

                            DLogIt("Results of procESPSetLastLibraryFetchDate: " & "ReturnValue=" & retvalue.ToString & " Message=" & errMess)

                        Catch ex As Exception

                            LogIt("Exception Trying procESPSetLastLibraryFetchDate: " & ex.Message)
                            AlertMail("ESP SetLastUpdate Failed.", "Exception Encounterd Setting Last Fetch Date " & ex.Message, "dbESPInfo")
                            ExLogIt("procESPSetLastLibraryFetchDate", "Exception Encounterd Setting Last Fetch Date " & ex.Message)
                            GetLibrariesBusyNow = False
                            Exit Sub

                        End Try
                    Else

                        LogIt("procESPMergeLibraries returned Non Zero")
                        AlertMail("ESP Merge Libraries Returned Non-Zero.", "Error Encounterd Merging Libraries " & "ReturnValue=" & retvalue.ToString & " Message=" & errMess, "dbESPInfo")
                        ExLogIt("procESPMergeLibraries", "Error Encounterd Merging Libraries " & "ReturnValue=" & retvalue.ToString & " Message=" & errMess)
                        GetLibrariesBusyNow = False
                        Exit Sub

                    End If

                Catch ex As Exception

                    LogIt("Exception While Trying to Update Table via procESPMergeLibraries: " & ex.Message)
                    AlertMail("ESP Merge Libraries Failed.", "Exception Encounterd Merging Libraries " & ex.Message, "dbESPInfo")
                    ExLogIt("procESPMergeLibraries", "Exception Encounterd Merging Libraries " & ex.Message)
                    GetLibrariesBusyNow = False
                    Exit Sub


                End Try


            Else

                'This is for debugging only when we want to just look at the response stream
                Dim enc As Encoding = System.Text.Encoding.GetEncoding(1252)
                Dim loResponseStream As New StreamReader(resp.GetResponseStream(), enc)

                Dim Response As String = loResponseStream.ReadToEnd()

                loResponseStream.Close()
                resp.Close()
                DLogIt(Response)


            End If

        Catch ex As Exception
            LogIt("Exception During Get Libraries: " & ex.Message.ToString() & ex.StackTrace.ToString)
            AlertMail("ESP Get Libraries Failed.", "Exception Encounterd Merging Libraries " & ex.Message, "beep")
            ExLogIt(uri, "Exception Encounterd Merging Libraries " & ex.Message)
            GetLibrariesBusyNow = False
            Exit Sub

        End Try

        GetLibrariesBusyNow = False
        GetLibrariesHasRun = True

        LogIt("ESP GetLibraries Completed. ESP Get Libraries Succeeded after " & GetLibrariesAttemptCount.ToString & " attempts.")

        GetLibrariesAttemptCount = 0


    End Sub

    Sub GetJobQueueItems()

        GetJobsBusyNow = True
        GetJobsAttemptCount += 1

        Dim content As String = ""
        Dim Method As String = "GET"

        Dim retvalue As Int32 = 0
        Dim errMess As String = ""


        Dim uri As String = My.Settings.ESPBaseURI & "jobs"

        DLogIt("JOBQ Calling: " & uri)

        Dim req As HttpWebRequest = TryCast(WebRequest.Create(uri), HttpWebRequest)
        req.KeepAlive = False
        req.Method = Method.ToUpper()

        req.Headers.Add("espKey", My.Settings.VendorKey)
        req.Headers.Add("api-version", My.Settings.APIVersion)

        Dim jsonSerializer As DataContractJsonSerializer
        Dim JobsQueue As JobsListJson
        Dim resp As HttpWebResponse
        Dim jsonSerializedString As String = ""
        Dim ctbladapter As New OrdersDataSetTableAdapters.QueriesTableAdapter
        Dim cretvalue As Int32 = 0
        Dim cerrMess As String = ""

        Dim where As Short = 0


        Try

            Try
                resp = TryCast(req.GetResponse(), HttpWebResponse)

            Catch ex As Exception
                LogIt("JOBQ Exception During Get Jobs HttpWebResponse: " & ex.Message.ToString())
                AlertMail("ESP Get Job Queue Items Failed.", "Exception Encounterd Creating HttpWebResponse " & ex.Message, "restESPInfo")
                ExLogIt(uri, "Exception Casting Get Jobs HttpWebResponse: " & ex.Message)
                GetJobsBusyNow = False
                Exit Sub

            End Try
            '===============================
            where = 1
            'Deserialize the json returned into the corresponding Class of LibraryData =========================================================================
            'created a serializer/deserializer for LibraryData
            jsonSerializer = New DataContractJsonSerializer(GetType(JobsListJson))
            where = 2
            'create a stream for getting the RESThttp response
            Dim iostrLibHttpResp As System.IO.Stream
            where = 3
            'load the response into the stream
            iostrLibHttpResp = resp.GetResponseStream()
            where = 4
            'now a reader to read the resulting stream into a String
            Dim sreader As New StreamReader(iostrLibHttpResp)
            where = 5
            '... and create the json string
            jsonSerializedString = sreader.ReadToEnd
            where = 6
            'then archive it for what it's worth, but only if there is at least 1 job.
            If jsonSerializedString.Length > 15 Then
                FileItUTF8(My.Settings.LogDir, "json", "JobQueue", jsonSerializedString)
            End If
            where = 7
            'now for the trick of saving the original json string and then using the same string it to deserialize
            '   NOTE the use of Encoding.Unicode.GetBytes to "fill" the stream with the string
            Dim memstrJsonString As New MemoryStream(Encoding.Unicode.GetBytes(jsonSerializedString))
            where = 8
            'Deserialize the json into Libraries collection
            JobsQueue = CType(jsonSerializer.ReadObject(memstrJsonString), JobsListJson)
            where = 9
            'and don't forget to clean up after yourself.
            sreader.Close()
            sreader.Dispose()

            memstrJsonString.Close()
            memstrJsonString.Dispose()

            iostrLibHttpResp.Close()
            iostrLibHttpResp.Dispose()

            jsonSerializer = Nothing

            DLogIt("GetJobs Json Response = " & jsonSerializedString)

            where = 10
            '===============================
            'This is for debugging only when we want to just look at the response stream before doing all the above stuff.
            'Dim enc As Encoding = System.Text.Encoding.GetEncoding(1252)
            'Dim loResponseStream As New StreamReader(resp.GetResponseStream(), enc)
            'where = 11
            'Dim Response As String = loResponseStream.ReadToEnd()
            'where = 12
            'loResponseStream.Close()
            'resp.Close()
            'where = 13
            'DLogIt("GetJobs ResponseStream= " & Response)

        Catch ex As Exception
            LogIt("JOBQ Exception During Get Job Items: " & ex.Message.ToString() & ex.StackTrace.ToString)
            AlertMail("ESP Get Jobs Failed.", "Exception Encounterd Updating ESPState with Jobs " & ex.Message, "beep")
            ExLogIt(uri, "Exception Encounterd Updating ESPState with Jobs " & ex.Message)
            GetJobsBusyNow = False
            Exit Sub

        End Try

        where = 19
        'Now load them into the utable and send it along....
        udtblESPStatus.Clear()
        udtblBranches.Clear()
        udtblFundCodes.Clear()


        Try
            where = 20
            DLogIt("JOBQ Jobs In Queue: " & JobsQueue.jobs.Count.ToString)
            where = 21
            For Each job As JobJson In JobsQueue.jobs
                'we aren't changing the ESPStateType, ErrorMessage, or passing json, hence the empty strings.
                udtblESPStatus.Rows.Add(job.cartId, Mid(job.jobType, 1, 4), DBNull.Value, job.jobId, job.status, DBNull.Value, DBNull.Value)
            Next
            where = 22
            ctbladapter.procESPSetESPState(udtblESPStatus, udtblBranches, udtblFundCodes, "OK")
            where = 23
            cretvalue = ctbladapter.GetReturnValue(5)
            where = 24
            cerrMess = ctbladapter.GetParam4(5).ToString

            where = 25
            LogIt("JOBQ Results of procESPSetESPState: " & "ReturnValue=" & cretvalue.ToString & " Message=" & cerrMess)
            where = 26
            If cretvalue = 0 Then
                DLogIt("JOBQ ESP Job Queue Udate SP Return value is good= " & retvalue.ToString)
            Else
                where = 27
                LogIt("JOBQ procESPSetESPState returned Non Zero")
                AlertMail("ESP Update Jobs Returned Non-Zero.", "Error Encounterd Updating ESP Queue " & "ReturnValue=" & cretvalue.ToString & " Message=" & cerrMess, "dbESPInfo")
                ExLogIt("procESPSetESPState", "Error Encounterd Updating Jobs " & "ReturnValue=" & cretvalue.ToString & " Message=" & cerrMess)
                GetJobsBusyNow = False
                Exit Sub

            End If

        Catch ex As Exception

            LogIt(where.ToString & ") JOBQ Get Jobs Exception While Trying to Update Table via procESPSetESPState: " & ex.Message)
            AlertMail("ESP Update Jobs Failed.", "Exception Encounterd Updating ESP Jobs " & ex.Message, "dbESPInfo")
            ExLogIt("procESPSetESPState", "Exception Encounterd Updating Jobs " & ex.Message)
            GetJobsBusyNow = False
            Exit Sub


        End Try



        GetJobsBusyNow = False

        LogIt("JOBQ ESP Get Job Queue Items Completed. ESP Get Jobs Succeeded after " & GetJobsAttemptCount.ToString & " attempts.")

        GetJobsAttemptCount = 0


    End Sub

    Function ESPCartRequestCount(type As String) As Integer
        Dim cmdIndex As Short = 0


        Select Case UCase(type)
            Case Is = "RANK"
                DoRankingsBusyNow = True
                cmdIndex = 1

            Case Is = "DIST"
                DoDistribsBusyNow = True
                cmdIndex = 3

            Case Is = "FUND"
                DoFundMonsBusyNow = True
                cmdIndex = 4

        End Select

        ESPCartRequestCount = 0
        Dim retvalue As Int32 = 0
        Dim errMess As String = ""
        Dim requestCount As Integer = 0

        Dim tbladaptr As New OrdersDataSetTableAdapters.QueriesTableAdapter

        Try
            Select Case UCase(type)
                Case Is = "RANK"
                    tbladaptr.procESPGetRankRequestCount(0, "OK")

                Case Is = "DIST"
                    tbladaptr.procESPGetDistributionRequestCount(0, "OK")

                Case Is = "FUND"
                    tbladaptr.procESPGetFundRequestCount(0, "OK")

            End Select
            'Now we need to read what the SP returned to us


            'Note that the below values are made accessible by creating a Partial Class related to the DataSet.  
            '   see OrdersDataSet.vb or AppWithSQLTests for details.
            retvalue = tbladaptr.GetReturnValue(cmdIndex)
            errMess = tbladaptr.GetParam2(cmdIndex)
            requestCount = tbladaptr.GetParam1(cmdIndex)

            ESPCartRequestCount = requestCount

            DLogIt("Results of GetRequestCount :" & type & " Message=" & errMess & " ReturnValue=" & retvalue.ToString & " Count: " & requestCount.ToString)

            If Not retvalue = 0 Then
                'Send an Alert that the SP returned an error.
                Dim AlertMess As String = "Error Calling SP procESPGetRequestCount-" & type & " Return value=" & retvalue.ToString & " Message=" & errMess
                AlertMail("GetCount Failed.", AlertMess, "dbESPInfo")
                LogIt("Error. Got Non Zero returnvalue. " & AlertMess)
                ExLogIt("procESPGetRequestCount", AlertMess)
                Select Case UCase(type)
                    Case Is = "RANK"
                        DoRankingsBusyNow = False

                    Case Is = "DIST"
                        DoDistribsBusyNow = False

                    Case Is = "FUND"
                        DoFundMonsBusyNow = False
                End Select
                Return 0
            End If

        Catch ex As Exception
            LogIt(type & " Exception Encountered Getting Count: " & ex.Message)
            AlertMail(type & " GetCount Failed.", "Exception Encounterd Getting Count " & ex.Message, "dbESPInfo")
            ExLogIt(type & " GetCount", "Exception Encountered Getting Count: " & ex.Message)
            Select Case UCase(type)
                Case Is = "RANK"
                    DoRankingsBusyNow = False

                Case Is = "DIST"
                    DoDistribsBusyNow = False

                Case Is = "FUND"
                    DoFundMonsBusyNow = False

            End Select
            Return 0
        End Try

        Select Case UCase(type)
            Case Is = "RANK"
                DoRankingsBusyNow = False

            Case Is = "DIST"
                DoDistribsBusyNow = False

            Case Is = "FUND"
                DoFundMonsBusyNow = False

        End Select

        Return ESPCartRequestCount

    End Function

    Sub DoRankings()

        DoRankingsBusyNow = True

        Dim tbladaptr As New OrdersDataSetTableAdapters.procESPGetRankRequestsTableAdapter
        Dim rnkCartsTable As New OrdersDataSet.procESPGetRankRequestsDataTable
        Dim retvalue As Int32 = 0
        Dim errMess As String = ""
        'we'll dummy up what we think is valid but empty XML to start
        Dim responseString As String = "<RankRequests></RankRequests>"
        Dim CurrentRequests As RankRequests
        Dim xmlSerializer As System.Xml.Serialization.XmlSerializer
        Dim xmlReader As System.Xml.XmlTextReader

        'Go get the carts to be ranked from SQL and fill the DataTable==========================================================================
        Try
            tbladaptr.Fill(rnkCartsTable, "OK")

            'this "schema" only represents the XML Blob as a single String value, so not useful as coded here.
            'rnkCartsTable.WriteXmlSchema(My.Settings.LogDir & "\CartsToRank.xsd")

            retvalue = tbladaptr.GetReturnValue(0)
            errMess = tbladaptr.GetParam1(0)

            If Not retvalue = 0 Then
                'Send an Alert that the SP returned an error.
                Dim AlertMess As String = "Error Calling SP procESPGetRankRequests." & " Return value=" & retvalue.ToString & " Message=" & errMess
                AlertMail("GetRankCarts Failed.", AlertMess, "dbESPInfo")
                LogIt("Error. Got Non Zero returnvalue. " & AlertMess)
                ExLogIt("procESPGetRankRequests", AlertMess)
                DoRankingsBusyNow = False
                Exit Sub
            End If


            'There should only be one "row" if successful
            For Each row As OrdersDataSet.procESPGetRankRequestsRow In rnkCartsTable.Rows
                responseString = row.NewRankRequests
            Next

            'Archive the requests in a single XML
            FileItUTF8(My.Settings.LogDir, "xml", "RankRequests", responseString)

            'TODO: if the string doesn't look right or is empty, Alert and Abort? (it may be safe to Archive the response, since we are setting it to dummy xml)

            'From here, we are in a Try so we just catch anywhere in the process and alert/abort as needed...
            xmlSerializer = New System.Xml.Serialization.XmlSerializer(GetType(RankRequests))
            DLogIt("created xml serializer")

            xmlReader = New System.Xml.XmlTextReader(New System.IO.StringReader(responseString))
            DLogIt("created xml reader")


            ' Use the Deserialize method to restore the object's state.
            CurrentRequests = CType(xmlSerializer.Deserialize(xmlReader), RankRequests)
            xmlReader.Close()

            DLogIt("excuted xml deserializer")

            DLogIt("Results of GetRankRequestCarts :" & " Message=" & errMess & " ReturnValue=" & retvalue.ToString & " 32 Bytes of Carts: " & responseString.Substring(0, 32) & "...")

            'LogIt("I got here")

            Try
                'This is a try all by itself because for some reason, doing IsNothing function on something that is, in fact, nothing throws and exception
                If Not IsNothing(CurrentRequests.RankRequest.Count) Then

                    'We have our Requested Carts so we go on to submit them for Ranking =================================================================
                    Try

                        LogIt("CurrentRequests.RankRequest.count = " & CurrentRequests.RankRequest.Count.ToString)
                        'OK, just to prove we populated the Class Object, just show the Lib IDs of the included carts.

                        If My.Settings.DebugLogging = True Then
                            For Each cart As RankRequestsRankRequest In CurrentRequests.RankRequest()
                                LogIt(cart.ESPLibraryID)
                            Next
                        End If

                        DLogIt("xmlString to RankRequests Class complete.")

                        'Now we handle each cart individually ===========================================================================================
                        For Each cart As RankRequestsRankRequest In CurrentRequests.RankRequest()
                            'First, save each individual cart as xml
                            Dim filprefix As String = "Cart2Rank_" & cart.CartID
                            Dim xmlFileName As String = FileCartUTF8(My.Settings.WorkingDir & "\", "xml", filprefix, cart, "RANK")

                            'Spawn off a RankIt(cart, attemptNo, delayseconds)
                            'Note that once all the carts have been passed to their own RankIt without exception, this Sub sets RankBusyNow to False and ends.
                            '   This does not mean Ranking, or Ranking Retries are not still going on, it just means that we're free to go pickup any new ones.
                            Dim RankIt As New DoACart(cart, 1, 0, xmlFileName, Me.udtblESPStatus, Me.udtblBranches, Me.udtblFundCodes, CartType.RANK, CartObject(2))

                            AddHandler RankIt.CallBackAlertMail, AddressOf CallBackAlertMail
                            AddHandler RankIt.CallBackFileItUTF8, AddressOf CallBackFileItUTF8
                            AddHandler RankIt.CallBackLogIt, AddressOf CallBackLogit
                            AddHandler RankIt.CallBackExLogIt, AddressOf CallBackExLogIt
                            AddHandler RankIt.CallBackCartAttemptComplete, AddressOf CallBackRankAttemptComplete

                        Next

                    Catch ex As Exception

                        LogIt("Exception Encountered While starting to Rank Carts: " & ex.Message & " I don't even know if I have cartIDs to fail or users to notify. I will probably just Alert.")
                        AlertMail("Exception during DoRankings: ", "Exception Encountered While starting to Rank Carts: " & ex.Message & " I don't even know if I have cartIDs to fail or users to notify. I will probably just Alert.", "beep")
                        ExLogIt("DoRankings", "Exception Encountered While starting to Rank Carts: " & ex.Message)

                    Finally
                        'cleanup
                        If Not IsNothing(xmlReader) Then
                            xmlReader.Close()
                            xmlReader = Nothing

                        End If
                    End Try
                Else
                    'we actually never get here because testing for IsNothing itself throws and exception
                    'so this action is never executed, and instead you end up in the below Catch
                    LogIt("CurrentRequests = Nothing!, but you'll never know it from here.")
                End If

            Catch ex As Exception

                LogIt("CurrentRequests Must Be = Nothing!: " & ex.Message)
                AlertMail("DoRanking Requests=Nothing? ", "CurrentRequests Appear To Be = Nothing!: " & ex.Message, "beep")
                ExLogIt("DoRankings", "CurrentRequests Must Be = Nothing!: " & ex.Message)

            End Try

            DLogIt("No Unhandled Exceptions During DoRankings.")

        Catch ex As Exception
            Dim errmessage As String = "Exception During DoRankings " & ex.Message
            If Not IsNothing(ex.InnerException) Then
                errmessage = errmessage & ex.InnerException.ToString
            End If
            LogIt(errmessage)
            ExLogIt("DoRankings", errmessage)
        End Try

        DoRankingsBusyNow = False


    End Sub

    Sub DoDistribs()
        DoDistribsBusyNow = True

        Dim tbladaptr As New OrdersDataSetTableAdapters.procESPGetDistributionRequestsTableAdapter
        Dim dstCartsTable As New OrdersDataSet.procESPGetDistributionRequestsDataTable
        Dim retvalue As Int32 = 0
        Dim errMess As String = ""
        'we'll dummy up what we think is valid but empty XML to start
        Dim responseString As String = "<DistributionRequests></DistributionRequests>"
        Dim CurrentRequests As DistributionRequests
        Dim xmlSerializer As System.Xml.Serialization.XmlSerializer
        Dim xmlReader As System.Xml.XmlTextReader

        Dim cartDistTimeoutInMinute As Short = Val(My.Settings.CartDistTimeoutInMinute)

        'Go get the carts to be  from SQL and fill the DataTable==========================================================================
        Try

            LogIt("DIST SQL timeout set as " & My.Settings.CartDistTimeoutInMinute & " minute(s)")

            tbladaptr.SetCommandTimeout(0, cartDistTimeoutInMinute * 60)
            tbladaptr.Fill(dstCartsTable, "OK")

            'this "schema" only represents the XML Blob as a single String value, so not useful as coded here.
            'rnkCartsTable.WriteXmlSchema(My.Settings.LogDir & "\CartsToRank.xsd")

            retvalue = tbladaptr.GetReturnValue(0)
            errMess = tbladaptr.GetParam1(0)

            If Not retvalue = 0 Then
                'Send an Alert that the SP returned an error.
                Dim AlertMess As String = "Error Calling SP procESPGetDistributionRequests." & " Return value=" & retvalue.ToString & " Message=" & errMess
                AlertMail("GetDistCarts Failed.", AlertMess, "dbESPInfo")
                LogIt("Error. Got Non Zero returnvalue. " & AlertMess)
                ExLogIt("procESPGetDistributionRequests", AlertMess)
                DoDistribsBusyNow = False
                Exit Sub
            End If


            'There should only be one "row" if successful
            For Each row As OrdersDataSet.procESPGetDistributionRequestsRow In dstCartsTable.Rows
                'TODO: name Column1 comes from stub code, see DoRankings and change as needed.
                responseString = row.NewDistributionRequests
            Next

            'Archive the requests in a single XML
            FileItUTF8(My.Settings.LogDir, "xml", "DistRequests", responseString)

            'TODO: if the string doesn't look right or is empty, Alert and Abort? (it may be safe to Archive the response, since we are setting it to dummy xml)

            'From here, we are in a Try so we just catch anywhere in the process and alert/abort as needed...
            xmlSerializer = New System.Xml.Serialization.XmlSerializer(GetType(DistributionRequests))
            DLogIt("created xml serializer")

            xmlReader = New System.Xml.XmlTextReader(New System.IO.StringReader(responseString))
            DLogIt("created xml reader")


            ' Use the Deserialize method to restore the object's state.
            CurrentRequests = CType(xmlSerializer.Deserialize(xmlReader), DistributionRequests)
            xmlReader.Close()

            DLogIt("excuted xml deserializer")

            DLogIt("Results of GetDistRequestCarts :" & " Message=" & errMess & " ReturnValue=" & retvalue.ToString & " 32 Bytes of Carts: " & responseString.Substring(0, 32) & "...")

            'LogIt("I got here")

            Try
                'This is a try all by itself because for some reason, doing IsNothing function on something that is, in fact, nothing throws and exception
                If Not IsNothing(CurrentRequests.DistributionRequest.Count) Then

                    'We have our Requested Carts so we go on to submit them for Distribution =================================================================
                    Try

                        LogIt("CurrentRequests.RankRequest.count = " & CurrentRequests.DistributionRequest.Count.ToString)
                        'OK, just to prove we populated the Class Object, just show the Lib IDs of the included carts.

                        If My.Settings.DebugLogging = True Then
                            For Each cart As DistributionRequestsDistributionRequest In CurrentRequests.DistributionRequest()
                                LogIt(cart.ESPLibraryID)
                            Next
                        End If

                        DLogIt("xmlString to DistributionRequests Class complete.")

                        'TODO - create DistIt.vb and all that.
                        'Now we handle each cart individually ===========================================================================================
                        For Each cart As DistributionRequestsDistributionRequest In CurrentRequests.DistributionRequest()
                            'First, save each individual cart as xml
                            Dim filprefix As String = "Cart2Dist_" & cart.CartID
                            Dim xmlFileName As String = FileCartUTF8(My.Settings.WorkingDir & "\", "xml", filprefix, cart, "DIST")

                            'Spawn off a RankIt(cart, attemptNo, delayseconds)
                            'Note that once all the carts have been passed to their own RankIt without exception, this Sub sets RankBusyNow to False and ends.
                            '   This does not mean Ranking, or Ranking Retries are not still going on, it just means that we're free to go pickup any new ones.
                            Dim DistIt As New DoACart(cart, 1, 0, xmlFileName, Me.udtblESPStatus, Me.udtblBranches, Me.udtblFundCodes, CartType.DIST, CartObject(0))

                            AddHandler DistIt.CallBackAlertMail, AddressOf CallBackAlertMail
                            AddHandler DistIt.CallBackFileItUTF8, AddressOf CallBackFileItUTF8
                            AddHandler DistIt.CallBackLogIt, AddressOf CallBackLogit
                            AddHandler DistIt.CallBackExLogIt, AddressOf CallBackExLogIt
                            AddHandler DistIt.CallBackCartAttemptComplete, AddressOf CallBackDistAttemptComplete

                        Next

                    Catch ex As Exception

                        LogIt("Exception Encountered While starting to Dist Carts: " & ex.Message & " I don't even know if I have cartIDs to fail or users to notify. I will probably just Alert.")
                        AlertMail("Exception during DoDistribs: ", "Exception Encountered While starting to Dist Carts: " & ex.Message & " I don't even know if I have cartIDs to fail or users to notify. I will probably just Alert.", "beep")
                        ExLogIt("DoDistribs", "Exception Encountered While starting to Dist Carts: " & ex.Message)

                    Finally
                        'cleanup
                        If Not IsNothing(xmlReader) Then
                            xmlReader.Close()
                            xmlReader = Nothing

                        End If
                    End Try
                Else
                    'we actually never get here because testing for IsNothing itself throws and exception
                    'so this action is never executed, and instead you end up in the below Catch
                    LogIt("CurrentRequests = Nothing!, but you'll never know it from here.")
                End If

            Catch ex As Exception

                LogIt("CurrentRequests Must Be = Nothing!: " & ex.Message)
                AlertMail("DoDistribs Requests=Nothing? ", "CurrentRequests Appear To Be = Nothing!: " & ex.Message, "beep")
                ExLogIt("DoDistribs", "CurrentRequests Must Be = Nothing!: " & ex.Message)

            End Try

            DLogIt("No Unhandled Exceptions During DoDistribs.")

        Catch ex As Exception
            Dim errmessage As String = "Exception During DoDistribs " & ex.Message
            If Not IsNothing(ex.InnerException) Then
                errmessage = errmessage & ex.InnerException.ToString
            End If
            LogIt(errmessage)
            ExLogIt("DoDistribs", errmessage)
            AlertMail("DoDistribs ProcessDistRequestCarts Failed", errmessage, "beep")
        End Try

        DoDistribsBusyNow = False

    End Sub

    Sub DoFundings()
        DoFundMonsBusyNow = True

        Dim tbladaptr As New OrdersDataSetTableAdapters.procESPGetFundRequestsTableAdapter
        Dim fndCartsTable As New OrdersDataSet.procESPGetFundRequestsDataTable
        Dim retvalue As Int32 = 0
        Dim errMess As String = ""
        'we'll dummy up what we think is valid but empty XML to start
        Dim responseString As String = "<FundRequests></FundRequests>"
        Dim CurrentRequests As FundRequests
        Dim xmlSerializer As System.Xml.Serialization.XmlSerializer
        Dim xmlReader As System.Xml.XmlTextReader

        'Go get the carts to be ranked from SQL and fill the DataTable==========================================================================
        Try
            tbladaptr.Fill(fndCartsTable, "OK")

            'this "schema" only represents the XML Blob as a single String value, so not useful as coded here.
            'rnkCartsTable.WriteXmlSchema(My.Settings.LogDir & "\CartsToRank.xsd")

            retvalue = tbladaptr.GetReturnValue(0)
            errMess = tbladaptr.GetParam1(0)

            If Not retvalue = 0 Then
                'Send an Alert that the SP returned an error.
                Dim AlertMess As String = "Error Calling SP procESPGetFundRequests." & " Return value=" & retvalue.ToString & " Message=" & errMess
                AlertMail("GetFundCarts Failed.", AlertMess, "dbESPInfo")
                LogIt("Error. Got Non Zero returnvalue. " & AlertMess)
                ExLogIt("procESPGetFundRequests", AlertMess)
                DoFundMonsBusyNow = False
                Exit Sub
            End If


            'There should only be one "row" if successful
            For Each row As OrdersDataSet.procESPGetFundRequestsRow In fndCartsTable.Rows
                'TODO: Column1 is due to stub, see DORankings.  May need to change once fully coded 
                responseString = row.NewFundRequests
            Next

            'Archive the requests in a single XML
            FileItUTF8(My.Settings.LogDir, "xml", "FundRequests", responseString)

            'TODO: if the string doesn't look right or is empty, Alert and Abort? (it may be safe to Archive the response, since we are setting it to dummy xml)

            'From here, we are in a Try so we just catch anywhere in the process and alert/abort as needed...
            xmlSerializer = New System.Xml.Serialization.XmlSerializer(GetType(FundRequests))
            DLogIt("created xml serializer")

            xmlReader = New System.Xml.XmlTextReader(New System.IO.StringReader(responseString))
            DLogIt("created xml reader")


            ' Use the Deserialize method to restore the object's state.
            CurrentRequests = CType(xmlSerializer.Deserialize(xmlReader), FundRequests)
            xmlReader.Close()

            DLogIt("excuted xml deserializer")

            DLogIt("Results of GetFundRequestCarts :" & " Message=" & errMess & " ReturnValue=" & retvalue.ToString & " 32 Bytes of Carts: " & responseString.Substring(0, 32) & "...")

            'LogIt("I got here")

            Try
                'This is a try all by itself because for some reason, doing IsNothing function on something that is, in fact, nothing throws and exception
                If Not IsNothing(CurrentRequests.FundRequest.Count) Then

                    'We have our Requested Carts so we go on to submit them for Ranking =================================================================
                    Try

                        LogIt("CurrentRequests.RankRequest.count = " & CurrentRequests.FundRequest.Count.ToString)
                        'OK, just to prove we populated the Class Object, just show the Lib IDs of the included carts.

                        If My.Settings.DebugLogging = True Then
                            For Each cart As FundRequestsFundRequest In CurrentRequests.FundRequest()
                                LogIt(cart.ESPLibraryID)
                            Next
                        End If

                        DLogIt("xmlString to RankRequests Class complete.")

                        'Now we handle each cart individually ===========================================================================================
                        For Each cart As FundRequestsFundRequest In CurrentRequests.FundRequest()
                            'First, save each individual cart as xml
                            Dim filprefix As String = "Cart2Fund_" & cart.CartID
                            Dim xmlFileName As String = FileCartUTF8(My.Settings.WorkingDir & "\", "xml", filprefix, cart, "FUND")

                            'Spawn off a RankIt(cart, attemptNo, delayseconds)
                            'Note that once all the carts have been passed to their own RankIt without exception, this Sub sets RankBusyNow to False and ends.
                            '   This does not mean Ranking, or Ranking Retries are not still going on, it just means that we're free to go pickup any new ones.
                            'TODO: Create FundIt.vb and adjust for FundRequests below
                            Dim FundIt As New DoACart(cart, 1, 0, xmlFileName, Me.udtblESPStatus, Me.udtblBranches, Me.udtblFundCodes, CartType.FUND, CartObject(1))

                            AddHandler FundIt.CallBackAlertMail, AddressOf CallBackAlertMail
                            AddHandler FundIt.CallBackFileItUTF8, AddressOf CallBackFileItUTF8
                            AddHandler FundIt.CallBackLogIt, AddressOf CallBackLogit
                            AddHandler FundIt.CallBackExLogIt, AddressOf CallBackExLogIt
                            AddHandler FundIt.CallBackCartAttemptComplete, AddressOf CallBackFundAttemptComplete


                        Next

                    Catch ex As Exception

                        LogIt("Exception Encountered While starting to Fund Carts: " & ex.Message & " I don't even know if I have cartIDs to fail or users to notify. I will probably just Alert.")
                        AlertMail("Exception during DoFundings: ", "Exception Encountered While starting to Fund Carts: " & ex.Message & " I don't even know if I have cartIDs to fail or users to notify. I will probably just Alert.", "beep")
                        ExLogIt("DoFundings", "Exception Encountered While starting to Fund Carts: " & ex.Message)

                    Finally
                        'cleanup
                        If Not IsNothing(xmlReader) Then
                            xmlReader.Close()
                            xmlReader = Nothing

                        End If
                    End Try
                Else
                    'we actually never get here because testing for IsNothing itself throws and exception
                    'so this action is never executed, and instead you end up in the below Catch
                    LogIt("CurrentRequests = Nothing!, but you'll never know it from here.")
                End If

            Catch ex As Exception

                LogIt("CurrentRequests Must Be = Nothing!: " & ex.Message)
                AlertMail("DoFunding Requests=Nothing? ", "CurrentRequests Appear To Be = Nothing!: " & ex.Message, "beep")
                ExLogIt("DoFundings", "CurrentRequests Must Be = Nothing!: " & ex.Message)

            End Try

            DLogIt("No Unhandled Exceptions During DoFundings.")

        Catch ex As Exception
            Dim errmessage As String = "Exception During DoFundings " & ex.Message
            If Not IsNothing(ex.InnerException) Then
                errmessage = errmessage & ex.InnerException.ToString
            End If
            LogIt(errmessage)
            ExLogIt("DoFundings", errmessage)
        End Try

        DoFundMonsBusyNow = False

    End Sub

    Sub PollForReprocessCarts(ByVal type As String)

        DLogIt("Entering Reprocessing for " & Trim(UCase(type)))

        If Trim(UCase(type)) = "RANK" Then

            DoRankingsBusyNow = True

            Dim xmlSerializer As System.Xml.Serialization.XmlSerializer = Nothing
            Dim xmlReader As System.Xml.XmlTextReader = Nothing
            Dim fs As IO.FileStream = Nothing
            Dim cart As RankRequestsRankRequest = Nothing
            Dim where As Short = 0
            Try

                For Each reprocFile As String In My.Computer.FileSystem.GetFiles(My.Settings.ReprocessRankDir)
                    Try
                        where = 1
                        fs = New IO.FileStream(reprocFile, IO.FileMode.Open)
                        where = 2
                        xmlReader = New System.Xml.XmlTextReader(fs)
                        xmlSerializer = New System.Xml.Serialization.XmlSerializer((GetType(RankRequestsRankRequest)))

                        where = 3
                        cart = CType(xmlSerializer.Deserialize(xmlReader), RankRequestsRankRequest)

                        where = 4
                        If Not IsNothing(cart) Then

                            where = 5
                            LogIt("Reprocessing " & reprocFile & " for ESPLibraryId = " & cart.ESPLibraryID)

                            'First, save each individual cart as xml
                            Dim filprefix As String = "Cart2Rank_" & cart.CartID
                            Dim xmlFileName As String = FileCartUTF8(My.Settings.WorkingDir & "\", "xml", filprefix, cart, "RANK")

                            'Spawn off a RankIt(cart, attemptNo, delayseconds)
                            'Note that once all the carts have been passed to their own RankIt without exception, this Sub sets RankBusyNow to False and ends.
                            '   This does not mean Ranking, or Ranking Retries are not still going on, it just means that we're free to go pickup any new ones.
                            where = 8
                            Dim RankIt As New DoACart(cart, 1, 0, xmlFileName, Me.udtblESPStatus, Me.udtblBranches, Me.udtblFundCodes, CartType.RANK, CartObject(2))

                            AddHandler RankIt.CallBackAlertMail, AddressOf CallBackAlertMail
                            AddHandler RankIt.CallBackFileItUTF8, AddressOf CallBackFileItUTF8
                            AddHandler RankIt.CallBackLogIt, AddressOf CallBackLogit
                            AddHandler RankIt.CallBackExLogIt, AddressOf CallBackExLogIt
                            AddHandler RankIt.CallBackCartAttemptComplete, AddressOf CallBackRankAttemptComplete

                        Else
                            LogIt("FileMethod: CurrentRequests = Nothing!")
                        End If

                        LogIt("Rank Reprocess handed to RankIt without exception.")

                    Catch ex As Exception
                        LogIt("Could Not Reprocess " & reprocFile & "due to exception at " & where.ToString & " :" & ex.Message)

                    Finally
                        If Not IsNothing(fs) Then
                            fs.Close()
                            fs.Dispose()
                        End If

                        If Not IsNothing(xmlReader) Then
                            xmlReader.Close()
                            xmlReader = Nothing
                        End If

                        Try
                            My.Computer.FileSystem.DeleteFile(reprocFile)

                        Catch ex As Exception
                            LogIt("Could Not Delete " & reprocFile & " :" & ex.Message)
                        End Try
                    End Try

                Next

            Catch ex As Exception
                'Send an Alert that poop happened.
                Dim AlertMess As String = "Exception Encountered during PollForReprocessRankCarts: " & ex.Message
                AlertMail("ReProcessRankCarts Failed.", AlertMess, "beep")
                LogIt(AlertMess)
                'ExLogIt("procESPGetRankRequests", AlertMess)
                DoRankingsBusyNow = False
                Exit Sub

            End Try

            DoRankingsBusyNow = False

        ElseIf Trim(UCase(type)) = "DIST" Then

            DoDistribsBusyNow = True

            Dim xmlSerializer As System.Xml.Serialization.XmlSerializer = Nothing
            Dim xmlReader As System.Xml.XmlTextReader = Nothing
            Dim fs As IO.FileStream = Nothing
            Dim cart As DistributionRequestsDistributionRequest = Nothing
            Dim where As Short = 0
            Try

                For Each reprocFile As String In My.Computer.FileSystem.GetFiles(My.Settings.ReprocessDistDir)
                    Try
                        where = 1
                        fs = New IO.FileStream(reprocFile, IO.FileMode.Open)
                        where = 2
                        xmlReader = New System.Xml.XmlTextReader(fs)
                        xmlSerializer = New System.Xml.Serialization.XmlSerializer((GetType(DistributionRequestsDistributionRequest)))

                        where = 3
                        cart = CType(xmlSerializer.Deserialize(xmlReader), DistributionRequestsDistributionRequest)

                        where = 4
                        If Not IsNothing(cart) Then

                            where = 5
                            LogIt("Reprocessing " & reprocFile & " for ESPLibraryId = " & cart.ESPLibraryID)

                            'First, save each individual cart as xml
                            Dim filprefix As String = "Cart2Dist_" & cart.CartID
                            Dim xmlFileName As String = FileCartUTF8(My.Settings.WorkingDir & "\", "xml", filprefix, cart, "DIST")

                            'Spawn off a RankIt(cart, attemptNo, delayseconds)
                            'Note that once all the carts have been passed to their own RankIt without exception, this Sub sets RankBusyNow to False and ends.
                            '   This does not mean Ranking, or Ranking Retries are not still going on, it just means that we're free to go pickup any new ones.
                            where = 8
                            Dim DistIt As New DoACart(cart, 1, 0, xmlFileName, Me.udtblESPStatus, Me.udtblBranches, Me.udtblFundCodes, CartType.DIST, CartObject(0))

                            AddHandler DistIt.CallBackAlertMail, AddressOf CallBackAlertMail
                            AddHandler DistIt.CallBackFileItUTF8, AddressOf CallBackFileItUTF8
                            AddHandler DistIt.CallBackLogIt, AddressOf CallBackLogit
                            AddHandler DistIt.CallBackExLogIt, AddressOf CallBackExLogIt
                            AddHandler DistIt.CallBackCartAttemptComplete, AddressOf CallBackDistAttemptComplete


                        Else
                            LogIt("FileMethod: CurrentRequests = Nothing!")
                        End If

                        LogIt("Distribution Reprocess handed to DistIt without exception.")

                    Catch ex As Exception
                        LogIt("Could Not Reprocess " & reprocFile & "due to exception at " & where.ToString & " :" & ex.Message)

                    Finally
                        If Not IsNothing(fs) Then
                            fs.Close()
                            fs.Dispose()
                        End If

                        If Not IsNothing(xmlReader) Then
                            xmlReader.Close()
                            xmlReader = Nothing
                        End If

                        Try
                            My.Computer.FileSystem.DeleteFile(reprocFile)

                        Catch ex As Exception
                            LogIt("Could Not Delete " & reprocFile & " :" & ex.Message)
                        End Try
                    End Try

                Next

            Catch ex As Exception
                'Send an Alert that poop happened.
                Dim AlertMess As String = "Exception Encountered during PollForReprocessDistCarts: " & ex.Message
                AlertMail("ReProcessDistCarts Failed.", AlertMess, "beep")
                LogIt(AlertMess)
                'ExLogIt("procESPGetRankRequests", AlertMess)
                DoDistribsBusyNow = False
                Exit Sub

            End Try

            DoDistribsBusyNow = False

        ElseIf Trim(UCase(type)) = "FUND" Then

            DoFundMonsBusyNow = True

            Dim xmlSerializer As System.Xml.Serialization.XmlSerializer = Nothing
            Dim xmlReader As System.Xml.XmlTextReader = Nothing
            Dim fs As IO.FileStream = Nothing
            Dim cart As FundRequestsFundRequest = Nothing
            Dim where As Short = 0
            Try

                For Each reprocFile As String In My.Computer.FileSystem.GetFiles(My.Settings.ReprocessFundDir)
                    Try
                        where = 1
                        fs = New IO.FileStream(reprocFile, IO.FileMode.Open)
                        where = 2
                        xmlReader = New System.Xml.XmlTextReader(fs)
                        xmlSerializer = New System.Xml.Serialization.XmlSerializer((GetType(FundRequestsFundRequest)))

                        where = 3
                        cart = CType(xmlSerializer.Deserialize(xmlReader), FundRequestsFundRequest)

                        where = 4
                        If Not IsNothing(cart) Then

                            where = 5
                            LogIt("Reprocessing " & reprocFile & " for ESPLibraryId = " & cart.ESPLibraryID)

                            'First, save each individual cart as xml
                            Dim filprefix As String = "Cart2Fund_" & cart.CartID
                            Dim xmlFileName As String = FileCartUTF8(My.Settings.WorkingDir & "\", "xml", filprefix, cart, "FUND")

                            'Spawn off a RankIt(cart, attemptNo, delayseconds)
                            'Note that once all the carts have been passed to their own RankIt without exception, this Sub sets RankBusyNow to False and ends.
                            '   This does not mean Ranking, or Ranking Retries are not still going on, it just means that we're free to go pickup any new ones.
                            where = 8
                            Dim FundIt As New DoACart(cart, 1, 0, xmlFileName, Me.udtblESPStatus, Me.udtblBranches, Me.udtblFundCodes, CartType.FUND, CartObject(1))

                            AddHandler FundIt.CallBackAlertMail, AddressOf CallBackAlertMail
                            AddHandler FundIt.CallBackFileItUTF8, AddressOf CallBackFileItUTF8
                            AddHandler FundIt.CallBackLogIt, AddressOf CallBackLogit
                            AddHandler FundIt.CallBackExLogIt, AddressOf CallBackExLogIt
                            AddHandler FundIt.CallBackCartAttemptComplete, AddressOf CallBackFundAttemptComplete


                        Else
                            LogIt("FileMethod: CurrentRequests = Nothing!")
                        End If

                        LogIt("Fund Reprocess handed to FundIt without exception.")

                    Catch ex As Exception
                        LogIt("Could Not Reprocess " & reprocFile & "due to exception at " & where.ToString & " :" & ex.Message)

                    Finally
                        If Not IsNothing(fs) Then
                            fs.Close()
                            fs.Dispose()
                        End If

                        If Not IsNothing(xmlReader) Then
                            xmlReader.Close()
                            xmlReader = Nothing
                        End If

                        Try
                            My.Computer.FileSystem.DeleteFile(reprocFile)

                        Catch ex As Exception
                            LogIt("Could Not Delete " & reprocFile & " :" & ex.Message)
                        End Try
                    End Try

                Next

            Catch ex As Exception
                'Send an Alert that poop happened.
                Dim AlertMess As String = "Exception Encountered during PollForReprocessFundCarts: " & ex.Message
                AlertMail("ReProcessFundCarts Failed.", AlertMess, "beep")
                LogIt(AlertMess)
                'ExLogIt("procESPGetRankRequests", AlertMess)
                DoFundMonsBusyNow = False
                Exit Sub

            End Try

            DoFundMonsBusyNow = False


        End If




    End Sub
    Sub DLogIt(mess As String)
        If My.Settings.DebugLogging = False Then Exit Sub

        LogIt(mess)

    End Sub
    Sub LogIt(mess As String)

        Dim ls As StreamWriter = Nothing
        Dim attempts As Short = 0

        'Note the literal service name below because InstallShield sets Me.ServiceName to Service1!

        Try
newattempt:

            ls = My.Computer.FileSystem.OpenTextFileWriter(My.Settings.LogDir & "\" & "ESPWinServices" & Format(Now, "yyyyMMdd") & ".txt", True, System.Text.Encoding.ASCII)
            ls.WriteLine(mess & " " & Now.ToString)
            ls.Close()
            ls = Nothing


        Catch ex As Exception

            If attempts >= 10 Then
                AlertMail("ESPWinServices - LogIt", "In " & attempts.ToString & " attempts could not LogIt: " & ex.Message, "beep")
                ls = My.Computer.FileSystem.OpenTextFileWriter(My.Settings.LogDir & "\" & "ESPWinServices" & Format(Now, "yyyyMMdd") & ".txt", True, System.Text.Encoding.ASCII)
                ls.WriteLine(ex.Message & " " & Now.ToString)
                ls.Close()
                ls = Nothing
                Exit Try
            Else
                'usually, we don't care of we can't log something, but an email alert might be warranted.
                If Not IsNothing(ls) Then
                    ls.Close()
                    ls = Nothing
                End If


                'Of course the better option is to just hold these entries in a class and the write them out on a timer,
                '   but this random generator staggers sleep times to hopefully avoid collisions between 2 calls in lockstep.
                Dim sleepytime As Int32 = GenRandomInt(1, 250)
                Thread.Sleep(sleepytime)

                GoTo newattempt

            End If


        Finally

            If Not IsNothing(ls) Then
                ls.Close()
                ls = Nothing

            End If

        End Try

    End Sub
    Function FileItUTF8(ByVal folder As String, ByVal extension As String, ByVal prefix As String, stuff As String) As String
        FileItUTF8 = ""
        Dim filnam As String = prefix & "_" & Format(Now, "yyyyMMddHHmmss") & "." & extension
        Dim ls As StreamWriter = Nothing

        'Note the literal service name below becuase InstallShield sets Me.ServiceName to Service1!
        Try
            ls = My.Computer.FileSystem.OpenTextFileWriter(folder & "\" & filnam, True, System.Text.Encoding.UTF8)
            ls.Write(stuff)
            ls.Close()
            ls = Nothing
            FileItUTF8 = filnam

        Catch ex As Exception
            'usually, we don't care of we can't log something, but an email alert might be warranted. 

            If Not IsNothing(ls) Then
                ls.Close()
                ls = Nothing
                FileItUTF8 = ""
            End If
        End Try

        Return FileItUTF8

    End Function

    Function FileCartUTF8(ByVal folder As String, ByVal extension As String, ByVal prefix As String, cart As Object, type As String) As String
        FileCartUTF8 = ""

        Dim ls As StreamWriter = Nothing
        Dim cartSerializer As System.Xml.Serialization.XmlSerializer = Nothing
        Dim xmlCart As String

        Select Case UCase(type)
            Case Is = "RANK"
                cartSerializer = New System.Xml.Serialization.XmlSerializer(GetType(RankRequestsRankRequest))
                xmlCart = "<RankRequestsRankRequest></RankRequestsRankRequest>"

            Case Is = "DIST"
                cartSerializer = New System.Xml.Serialization.XmlSerializer(GetType(DistributionRequestsDistributionRequest))
                xmlCart = "<DistributionRequestsDistributionRequest></DistributionRequestsDistributionRequest>"

            Case Is = "FUND"
                cartSerializer = New System.Xml.Serialization.XmlSerializer(GetType(FundRequestsFundRequest))
                xmlCart = "<FundRequestsFundRequest></FundRequestsFundRequest>"

            Case Else
                'TODO SOMETHING BAD HAPPENED
                'Throw something to be caught somewhere or if cartSerializer below is Nothing, throw it there
        End Select

        Dim msCart As MemoryStream = Nothing
        Dim msr As StreamReader = Nothing


        Try
            msCart = New MemoryStream

            'Serialize generally writes to a file but a MemoryStream is an unpublished alternative
            cartSerializer.Serialize(msCart, cart)

            'set the pointer back to the beginning for reading it back
            msCart.Position = 0

            'create the reader to read the stream into the string
            msr = New StreamReader(msCart)

            'and finally read the stream into the jsonString
            xmlCart = msr.ReadToEnd

            FileCartUTF8 = FileItUTF8(folder, extension, prefix, xmlCart)

        Catch ex As Exception
            LogIt("Could not Serialize Cart:" & cart.CartID & " - " & ex.Message)
            FileCartUTF8 = ""
        Finally
            If Not IsNothing(msCart) Then
                msCart.Close()
                msCart = Nothing
            End If

            If Not IsNothing(msr) Then
                msr.Close()
                msr = Nothing
            End If
        End Try

        Return FileCartUTF8

    End Function


    Private Sub PollTimer_Elapsed(sender As Object, e As Timers.ElapsedEventArgs) Handles GetLibPollTimer.Elapsed
        If Not My.Settings.LibraryEnablePolling Then Exit Sub

        Dim ranges As String() = My.Settings.GetLibrariesMinuteRange.Split(",")

        Dim startRange As String = ranges(0)
        Dim endRange As String = ranges(1)

        If (
            Convert.ToInt32(Strings.Format(Now, "mm")) >= Convert.ToInt32(startRange) And
            Convert.ToInt32(Strings.Format(Now, "mm")) <= Convert.ToInt32(endRange)) Then

            If GetLibrariesHasRun Then
                Exit Sub
            Else
                If GetLibrariesAttemptCount < My.Settings.GetLibrariesRetryCount Then
                    GetLibraries()
                Else
                    AlertMail("ESP Get Libraries Failed for hourly job " + Strings.Format(Now, "hh tt"), "ESP Get Libraries could not complete without errors/exceptions in " & GetLibrariesAttemptCount.ToString & " Attempts.  It will not be retried.", "beep")
                    GetLibrariesHasRun = True
                    GetLibrariesAttemptCount = 0
                End If
            End If


        Else
            GetLibrariesHasRun = False
        End If


    End Sub

    Sub LoadAppData()
        ' Create an instance of the XmlSerializer specifying type and namespace.
        Dim serializer As New System.Xml.Serialization.XmlSerializer(GetType(AppData))


        ' A FileStream is needed to read the XML document.
        'Note that there is no Application.StartupPath because there is no Application object for a Windows Service.
        'for a Forms application, it would be:
        '   Dim fs As New IO.FileStream(Application.StartupPath & "\AutoFTPTrayAppData.xml", IO.FileMode.Open)
        Dim fs As New IO.FileStream(MyAppDomain.BaseDirectory & "\ESPWinServicesAppData.xml", IO.FileMode.Open)

        Dim reader As New System.Xml.XmlTextReader(fs)

        ' Use the Deserialize method to restore the object's state.
        MyAppData = CType(serializer.Deserialize(reader), AppData)

        'For Each recip As AppDataEmailRecipient In MyAppData.EmailRecipients
        '    For Each grp As String In recip.Group
        '        MsgBox(recip.EmailAddress & " " & grp)
        '    Next
        'Next

        fs.Close()
        reader.Close()
        fs = Nothing
        reader = Nothing

    End Sub

    Sub AlertMail(ByVal subject As String, ByVal message As String, ByVal group As String)

        'Stop

        subject = subject & " - " & MachineName & "(" & My.Settings.Environment & ")"

        'We are doing this stand alone from here rather than using MailMan.
        'it remains to be seen if we need to prevent hordes of emails with a "lastnotified" check.

        'If DateDiff(DateInterval.Minute, LastAlert, Now) < 5 Then Exit Sub
        Try
            Select Case UCase(group)

                Case Is = "BEEP", "DBESPALERT", "RESTESPALERT"
                    For Each Member As AppDataEmailRecipient In MyAppData.EmailRecipients
                        For Each recipgroup As String In Member.Group
                            If UCase(recipgroup) = UCase(group) Then

                                Dim recip As New MailAddress(Member.EmailAddress, Member.Name)
                                Dim sendr As New MailAddress("PageMan@baker-taylor.com", "PageMan")
                                Dim SMTPMess As New MailMessage(sendr, recip)
                                Dim SMTPMail As New SmtpClient(My.Settings.SMTPServer)


                                SMTPMess.Subject = subject
                                SMTPMess.Body = message

                                If My.Settings.SMTPAuth Then
                                    Dim mycreds As New System.Net.NetworkCredential(My.Settings.SMTPUser, My.Settings.SMTPPass)
                                    SMTPMail.UseDefaultCredentials = False
                                    SMTPMail.Credentials = mycreds
                                End If


                                SMTPMail.Send(SMTPMess)
                                SMTPMess = Nothing
                                SMTPMail = Nothing

                            End If

                        Next
                    Next

                Case Else
                    For Each member As AppDataEmailRecipient In MyAppData.EmailRecipients
                        For Each recipgroup As String In member.Group
                            If UCase(recipgroup) = UCase(group) Then


                                Dim recip As New MailAddress(member.EmailAddress, member.Name)
                                Dim sendr As New MailAddress("MailMan@baker-taylor.com", "MailMan")
                                Dim SMTPMess As New MailMessage(sendr, recip)
                                Dim SMTPMail As New SmtpClient(My.Settings.SMTPServer)

                                SMTPMess.Subject = subject
                                SMTPMess.Body = message

                                If My.Settings.SMTPAuth Then
                                    Dim mycreds As New System.Net.NetworkCredential(My.Settings.SMTPUser, My.Settings.SMTPPass)
                                    SMTPMail.UseDefaultCredentials = False
                                    SMTPMail.Credentials = mycreds
                                End If

                                SMTPMail.Send(SMTPMess)
                                SMTPMess = Nothing
                                SMTPMail = Nothing

                            End If
                        Next
                    Next

            End Select



        Catch ex As Exception
            'Golly, if I can't send alerts, alls I can dos os log the bastard.
            LogIt("ERROR While Attempting Alert Email : " & ex.Message)

        End Try

        'LastAlert = Now
    End Sub

    Private Sub CartJobsPollTimer_Elapsed(sender As Object, e As Timers.ElapsedEventArgs) Handles CartJobsPollTimer.Elapsed
        If Not My.Settings.ESPCartsEnabled Then Exit Sub
        If ESPCartPickupOverride Then Exit Sub


        If Not DoRankingsBusyNow Then
            Dim curRequestCount As Integer = ESPCartRequestCount("RANK")

            If curRequestCount > 0 Then
                LogIt("RankRequestCount is over zero: " & curRequestCount.ToString & " ... starting DoRankings!")
                DoRankings()
            Else
                DLogIt("CartsToRank is zero or less: " & curRequestCount.ToString)
            End If

            PollForReprocessCarts("RANK")

        End If

        If Not DoDistribsBusyNow Then
            Dim curRequestCount As Integer = ESPCartRequestCount("DIST")

            If curRequestCount > 0 Then
                LogIt("DistRequestCount is over zero: " & curRequestCount.ToString & " ... starting DoDistribs!")
                DoDistribs()
            Else
                DLogIt("CartsToDist is zero or less: " & curRequestCount.ToString)
            End If

            PollForReprocessCarts("DIST")

        End If

        If Not DoFundMonsBusyNow Then
            Dim curRequestCount As Integer = ESPCartRequestCount("FUND")

            If curRequestCount > 0 Then
                LogIt("FundRequestCount is over zero: " & curRequestCount.ToString & " ... starting DoFundings!")
                DoFundings()
            Else
                DLogIt("CartsToFund is zero or less: " & curRequestCount.ToString)
            End If

            PollForReprocessCarts("FUND")

        End If

        'for testing in one shot mode we only pickup once to disable Polling for Carts and calling for Jobs
        'ESPCartPickupOverride = True

        If Not GetJobsBusyNow Then
            GetJobQueueItems()
        End If

    End Sub

    Sub CallBackLogit(ByVal sender As Object, ByVal e As LogItEventArgs)
        LogIt(e.mess)
    End Sub

    Sub CallBackAlertMail(ByVal sender As Object, ByVal e As AlertMailEventArgs)
        AlertMail(e.subject, e.body, e.group)
    End Sub

    Sub CallBackFileItUTF8(ByVal sender As Object, ByVal e As FileItUTF8EventArgs)
        FileItUTF8(e.dirPath, e.extension, e.filePrefix, e.content)
    End Sub

    Sub CallBackExLogIt(ByVal sender As Object, ByVal e As ExLogItEventArgs)
        ExLogIt(e.method, e.exMessage, e.RequestMsg, e.ResponseMsg)
    End Sub

    Sub CallBackRankAttemptComplete(ByVal sender As Object, ByVal rc As CartAttemptCompleteEventArgs)
        'All logging and notifications are done, so all we care about is if this guy should be tried again.
        Dim recount As Short = Val(My.Settings.CartJobsRetryCount)
        Dim redelay As Short = Val(My.Settings.CartJobsRetryIntervalSecs)

        If rc.enableRetry Then
            If rc.attemptNo < (recount + 1) Then
                Dim RankIt As New DoACart(rc.cart, rc.attemptNo + 1, redelay, rc.xmlFileName, Me.udtblESPStatus, Me.udtblBranches, Me.udtblFundCodes, CartType.RANK, CartObject(2))

                AddHandler RankIt.CallBackAlertMail, AddressOf CallBackAlertMail
                AddHandler RankIt.CallBackFileItUTF8, AddressOf CallBackFileItUTF8
                AddHandler RankIt.CallBackLogIt, AddressOf CallBackLogit
                AddHandler RankIt.CallBackExLogIt, AddressOf CallBackExLogIt
                AddHandler RankIt.CallBackCartAttemptComplete, AddressOf CallBackRankAttemptComplete
            Else
                LogIt("RANK Retries Exhausted for Cart " & rc.cart.CartID)
                'move the xml file if done with retries
                My.Computer.FileSystem.MoveFile(My.Settings.WorkingDir & "\" & rc.xmlFileName, My.Settings.LogDir & "\" & rc.xmlFileName)

            End If
        Else
            'move the xml file if no retries
            My.Computer.FileSystem.MoveFile(My.Settings.WorkingDir & "\" & rc.xmlFileName, My.Settings.LogDir & "\" & rc.xmlFileName)

        End If

    End Sub

    Sub CallBackDistAttemptComplete(ByVal sender As Object, ByVal rc As CartAttemptCompleteEventArgs)
        'All logging and notifications are done, so all we care about is if this guy should be tried again.
        Dim recount As Short = Val(My.Settings.CartJobsRetryCount)
        Dim redelay As Short = Val(My.Settings.CartJobsRetryIntervalSecs)

        If rc.enableRetry Then
            If rc.attemptNo < (recount + 1) Then
                Dim DistIt As New DoACart(rc.cart, rc.attemptNo + 1, redelay, rc.xmlFileName, Me.udtblESPStatus, Me.udtblBranches, Me.udtblFundCodes, CartType.DIST, CartObject(0))

                AddHandler DistIt.CallBackAlertMail, AddressOf CallBackAlertMail
                AddHandler DistIt.CallBackFileItUTF8, AddressOf CallBackFileItUTF8
                AddHandler DistIt.CallBackLogIt, AddressOf CallBackLogit
                AddHandler DistIt.CallBackExLogIt, AddressOf CallBackExLogIt
                AddHandler DistIt.CallBackCartAttemptComplete, AddressOf CallBackDistAttemptComplete

            Else
                LogIt("DIST Retries Exhausted for Cart " & rc.cart.CartID)
                'move the xml file if done with retries
                My.Computer.FileSystem.MoveFile(My.Settings.WorkingDir & "\" & rc.xmlFileName, My.Settings.LogDir & "\" & rc.xmlFileName)

            End If
        Else
            'move the xml file if no retries
            My.Computer.FileSystem.MoveFile(My.Settings.WorkingDir & "\" & rc.xmlFileName, My.Settings.LogDir & "\" & rc.xmlFileName)

        End If

    End Sub

    Sub CallBackFundAttemptComplete(ByVal sender As Object, ByVal rc As CartAttemptCompleteEventArgs)
        'All logging and notifications are done, so all we care about is if this guy should be tried again.
        Dim recount As Short = Val(My.Settings.CartJobsRetryCount)
        Dim redelay As Short = Val(My.Settings.CartJobsRetryIntervalSecs)

        If rc.enableRetry Then
            If rc.attemptNo < (recount + 1) Then
                Dim FundIt As New DoACart(rc.cart, rc.attemptNo + 1, redelay, rc.xmlFileName, Me.udtblESPStatus, Me.udtblBranches, Me.udtblFundCodes, CartType.FUND, CartObject(1))

                AddHandler FundIt.CallBackAlertMail, AddressOf CallBackAlertMail
                AddHandler FundIt.CallBackFileItUTF8, AddressOf CallBackFileItUTF8
                AddHandler FundIt.CallBackLogIt, AddressOf CallBackLogit
                AddHandler FundIt.CallBackExLogIt, AddressOf CallBackExLogIt
                AddHandler FundIt.CallBackCartAttemptComplete, AddressOf CallBackFundAttemptComplete

            Else
                LogIt("FUND Retries Exhausted for Cart " & rc.cart.CartID)
                'move the xml file if done with retries
                My.Computer.FileSystem.MoveFile(My.Settings.WorkingDir & "\" & rc.xmlFileName, My.Settings.LogDir & "\" & rc.xmlFileName)

            End If
        Else
            'move the xml file if no retries
            My.Computer.FileSystem.MoveFile(My.Settings.WorkingDir & "\" & rc.xmlFileName, My.Settings.LogDir & "\" & rc.xmlFileName)

        End If

    End Sub

    Private Function GenRandomInt(min As Int32, max As Int32) As Int32
        'This, from posts, is not absolutely bullet proof, but will not be a problem in the controlled instance where it will be used.

        'The sleep prevents the system clock from allowing the same number to be returned for calls made in rapid succession
        Thread.Sleep(1)
        'The static allows successive numbers rather than a new generator with every call
        Static staticRandomGenerator As New System.Random
        'Adding +1 to the max value allows for the fact that the function, as designed, never returns the max value
        Return staticRandomGenerator.Next(min, max + 1)

    End Function

    Sub ExLogIt(ByVal method As String, ByVal exMessage As String, Optional ByVal requestMsg As String = "Request Message Not Included.", Optional ByVal responseMsg As String = "Respons Message Not Included", Optional ByVal vendorKey As String = "", Optional ByVal createdBy As String = "ESPWinServices")
        If vendorKey = "" Then vendorKey = My.Settings.VendorKey
        requestMsg = String.Format("api-version:{0} ", My.Settings.APIVersion) & requestMsg

        'To Limit the number of fields that need to be created when calling, note the optional elements and default values above.  A variable can't be used, so we convert a blank vendorKey to the settings value.

        'TODO: Start Inserting the calls to this with the other LogIts and AlertMails in all the Exception/Error traps.

        Dim tbladapter As New ExceptionLoggingDataSetTableAdapters.QueriesTableAdapter
        Dim retvalue As Int32 = 0
        Dim errMess As String = ""

        Try
            tbladapter.procTS360APILogRequests(method, requestMsg, responseMsg, vendorKey, Now, createdBy, exMessage)
            retvalue = tbladapter.GetReturnValue(0)
            errMess = tbladapter.GetParam1(0)

            If Not retvalue = 0 Then
                'Send an Alert that the SP returned an error.
                Dim AlertMess As String = "Error Calling SP procTS360APILogRequests. Return value=" & retvalue.ToString & " Message=" & errMess
                AlertMail("procTS360APILogRequests Failed.", AlertMess, "dbESPInfo")
                LogIt("Error. Got Non Zero returnvalue. " & AlertMess)
            Else
                DLogIt("Results of procTS360APILogRequests:  Return value=" & retvalue.ToString & " Message=" & errMess)
            End If

        Catch ex As Exception
            'Send an Alert that the SP returned an error.
            Dim AlertMess As String = "Exception during ExLogIt: " & ex.Message
            AlertMail("ExLogIt Failed.", AlertMess, "mail")
            LogIt(AlertMess)

        End Try


    End Sub
End Class
