<?xml version="1.0" encoding="utf-8"?>
<!-- DWXMLSource="9786307247669 NEW NAMESPACE.xml" -->
<!DOCTYPE xsl:stylesheet  [
  <!ENTITY nbsp   "&#160;">
  <!ENTITY copy   "&#169;">
  <!ENTITY reg    "&#174;">
  <!ENTITY trade  "&#8482;">
  <!ENTITY mdash  "&#8212;">
  <!ENTITY ldquo  "&#8220;">
  <!ENTITY rdquo  "&#8221;">
  <!ENTITY pound  "&#163;">
  <!ENTITY yen    "&#165;">
  <!ENTITY euro   "&#8364;">
]>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:m="http://ContentCafe2.btol.com">
  <xsl:output method="html" encoding="utf-8"/>
  <!-- ********************************************************************************* -->
  <!-- Set Variables that are used as gates inside some templates -->
  <!--	<xsl:variable name="DoVideo"><xsl:if test="(//m:VideoRelease)">1</xsl:if></xsl:variable>
	<xsl:variable name="DoCinema"><xsl:if test="(//m:SimilarCinema/m:SimilarCinemaRelease)">1</xsl:if></xsl:variable>
-->
  <xsl:variable name="DoPopular">
    <xsl:if test="(//m:PopularMusic)">1</xsl:if>
  </xsl:variable>
  <xsl:variable name="DoSongs">
    <xsl:if test="(//m:Songs)">1</xsl:if>
  </xsl:variable>
  <xsl:variable name="DoArtists">
    <xsl:if test="(//m:EssentialArtist)">1</xsl:if>
  </xsl:variable>
  <xsl:variable name="DoClassical">
    <xsl:if test="(//m:ClassicalMusic)">1</xsl:if>
  </xsl:variable>
  <!--	<xsl:variable name="DoGames"><xsl:if test="(//m:GameRelease)">1</xsl:if></xsl:variable> 
-->
  <!-- Main Template applies applicable Templates in sequence when data is present -->
  <xsl:template match="/">
    <xsl:apply-templates select="//m:VideoRelease"/>
    <xsl:apply-templates select="//m:PopularMusic"/>
    <xsl:apply-templates select="//m:ClassicalMusic"/>
    <!-- Removed for ORS on 1/17/2010 by RCW  since we do not sell Games anymore.
    <xsl:apply-templates select="//m:GameRelease"/>
-->
  </xsl:template>

  <!-- ********************************************************************************* -->
  <!-- The test for position #1 eliminates multiple nodes that are present (2 VideRelease, etc.) -->
  <xsl:template match="//m:VideoRelease">
    <xsl:if test="position() =1">
      <!--        <xsl:value-of select="./m:ReleaseFormat"/> : <xsl:value-of select="./@ID"/> : 
        <xsl:value-of select="./@MuzeID"/> : <xsl:value-of select="./@MuzeRefNum"/>
         : <xsl:value-of select="./@xmls"/><xsl:if test="position() =1">ONE</xsl:if>
         <xsl:if test="position() =2">TWO</xsl:if> : <xsl:value-of select="./m:UPC"/><br/>
-->
      <xsl:call-template name="Video"/>
    </xsl:if>
  </xsl:template>

  <xsl:template match="//m:PopularMusic">
    <xsl:if test="position() =1">
      <xsl:if test="$DoPopular = '1' or $DoSongs = '1' or $DoArtists = '1'">
        <xsl:call-template name="PopMusic"/>
      </xsl:if>
    </xsl:if>
  </xsl:template>

  <xsl:template match="//m:ClassicalMusic">
    <xsl:if test="position() =1">
      <xsl:call-template name="ClassMusic"/>
    </xsl:if>
  </xsl:template>

  <xsl:template match="//m:GameRelease">
    <xsl:if test="position() =1">
      <xsl:call-template name="Games"/>
    </xsl:if>
  </xsl:template>
  <!-- ********************************************************************************* -->


  <!-- ********* SECTION ONE is VideoRelease with SimilarCinemas *********************** -->
  <xsl:template name="Video" match="m:VideoRelease">
    <!-- START: VideoRelease Content ***************************************************** -->
    <!-- <xsl:if test="count(//VideoRelease) > '0' or count(//SimilarCinemaRelease) > '0'"> -->
    <div>
        <img src="/_layouts/IMAGES/CSDefaultSite/assets/images/Rovi_Logo_Hero_Black_RGB.JPG" style="height:40px;width:66px" />
        <p class="rovitext">
          Portions of Content Provided by Rovi Corporation. &copy;&nbsp;[copyrightyear] Rovi Corporation
        </p>
    </div>
    <br/>
    <table class="Muze" style="width:600px !important;">
      <tr>
        <!-- *********** START of MuzeLeft COLUMN **************************************** -->
        <td class="MuzeLeft">
          <!-- ************** START of VideoRelease Table*********************************** -->
          <!-- <xsl:if test="$DoVideo = '1'"> -->
          <h2>Detailed Product Description: Video</h2>
          <table  class="MuzeLeft">
            <xsl:call-template name="inprint"/>
            <xsl:call-template name="runtime"/>
            <xsl:call-template name="audio"/>
            <xsl:call-template name="feature"/>
            <xsl:call-template name="rating"/>
            <xsl:call-template name="extrainfo"/>
            <xsl:call-template name="category"/>
            <xsl:call-template name="silent"/>
            <xsl:call-template name="origrelease"/>
            <xsl:call-template name="releasecompany"/>
            <xsl:call-template name="catalog"/>
            <xsl:call-template name="area"/>
          </table>
          <!-- </xsl:if> -->
          <!-- ************** END of VideoRelease Table************************************* -->

          <!-- *********************** START of Awards Table ******************************* -->
          <xsl:if test="(./m:VideoProduct/m:Awards/m:Award/m:AwardGivrName)">
            <h2>Awards</h2>
            <table class="MuzeLeft">
              <xsl:call-template name="award"/>
            </table>
          </xsl:if>
          <!-- *********************** END of Awards Table ********************************* -->

          <!-- *********************** START of Contributors Table ************************* -->
          <xsl:if test="(./m:VideoProduct/m:ProdContribJobs/m:ProdContribJob)">
            <h2>Contributors</h2>
            <table class="MuzeLeft">
              <xsl:call-template name="contributor"/>
            </table>
          </xsl:if>
          <!-- *********************** END of Contributors Table *************************** -->

          <!-- ******************* START of Commentary and Reviews Table ******************* -->
          <xsl:if test="(./m:VideoProduct/m:NoteForProducts/m:NoteForProduct)">
            <h2>Commentary and Reviews</h2>
            <table class="MuzeLeft">
              <xsl:call-template name="synopsis"/>
              <xsl:call-template name="releasenote"/>
              <xsl:call-template name="reviews"/>
            </table>
          </xsl:if>
          <!-- *********************** END of NoteForProducts Section ********************** -->
        </td>
        <!-- ************* END of MuzeLeft COLUMN **************************************** -->

        <!-- *********** START of MuzeRight COLUMN *************************************** -->
        <td class="MuzeRight">

          <xsl:if test="(../m:SimilarCinema[position()=1]/m:SimilarCinemaRelease)">
            <xsl:call-template name="Cinema"/>
          </xsl:if>
        </td>
        <!-- ************ END of MuzeRight COLUMN **************************************** -->
      </tr>
    </table>
    <!-- </xsl:if> -->
    <!-- END: VideoRelease Content ******************************************************* -->
  </xsl:template>

  <xsl:template name="Cinema">
    <!-- *********** START of the SimilarCinema Section ****************************** -->
    <!--
    <xsl:if test="$DoCinema = '1'">
    <xsl:for-each select="//SimilarCinema">
    -->
    <!--  Following line was removed for ORS on 1/17/2011 to prevent search link for All Similar Cinema RCW
            <xsl:call-template name="allsimilar"/>
  -->
    <xsl:call-template name="similartitle"/>
    <!-- 	</xsl:for-each>
            </xsl:if>
    -->
    <!-- ***************************************************************************** -->
    <!-- ***************************************************************************** -->
    <!-- ************* End of the SimilarCinema Section ****************************** -->
    <!-- ***************************************************************************** -->
    <!-- ***************************************************************************** -->
  </xsl:template>

  <xsl:template name="inprint">
    <!-- START of InPrintDate Section ******* -->
    <xsl:if test="(./m:InPrintDate)">
      <tr>
        <td class="label" >In Print Date: </td>
        <td class="value2" >
          <xsl:value-of select="./m:InPrintDate"/>
        </td>
      </tr>
    </xsl:if>
    <!-- END of InPrintDate Section **************************** -->
  </xsl:template>

  <xsl:template name="runtime">
    <!-- START of Runtime Section ********** -->
    <xsl:if test="(./m:Runtime) >  0">
      <tr>
        <td class="label" >Run Time: </td>
        <td class="value2" >
          <xsl:value-of select='format-number(./m:Runtime div 60, "#")'/> Minutes
        </td>
      </tr>
    </xsl:if>
    <!-- END of Runtime Section ******************************** -->
  </xsl:template>

  <xsl:template name="audio">
    <!-- START of Audio Section ************* -->
    <xsl:variable name="AudioData">
      <xsl:if test="(./m:HiFiSound) != 'false'">1</xsl:if>
      <xsl:if test="(./m:SurroundSound) != 'false'">2</xsl:if>
      <xsl:if test="(./m:DigitalSound) != 'false'">3</xsl:if>
      <xsl:if test="(./m:StereoSound) != 'false'">4</xsl:if>
      <xsl:if test="(./m:CXEncoding) != 'false'">5</xsl:if>
      <xsl:if test="(./m:AC3Sound) != 'false'">6</xsl:if>
      <xsl:if test="(./m:THXSound) != 'false'">7</xsl:if>
      <xsl:if test="(./m:DTSStereo) != 'false'">8</xsl:if>
    </xsl:variable>
    <xsl:if test="$AudioData != ''">
      <tr>
        <td class="label" >Audio: </td>
        <td class="value2" >
          <xsl:if test="(./m:HiFiSound) != 'false'">
            HiFi Sound<br/>
          </xsl:if>
          <xsl:if test="(./m:SurroundSound) != 'false'">
            Surround Sound<br/>
          </xsl:if>
          <xsl:if test="(./m:DigitalSound) != 'false'">
            Digital Sound<br/>
          </xsl:if>
          <xsl:if test="(./m:StereoSound) != 'false'">
            Stereo Sound<br/>
          </xsl:if>
          <xsl:if test="(./m:CXEncoding) != 'false'">
            CX Encoding<br/>
          </xsl:if>
          <xsl:if test="(./m:AC3Sound) != 'false'">
            AC3 Sound<br/>
          </xsl:if>
          <xsl:if test="(./m:THXSound) != 'false'">
            THX Sound<br/>
          </xsl:if>
          <xsl:if test="(./m:DTSStereo) != 'false'">
            DTS Stereo<br/>
          </xsl:if>
          <xsl:value-of select="$AudioData"/>
        </td>
      </tr>
    </xsl:if>
    <tr>
      <td class="label" >Recording Mode</td>
      <td class="value2" >
        <xsl:value-of select="m:RecordingMode"/>
      </td>
    </tr>
    <tr>
      <td class="label" >Noise Reduction</td>
      <td class="value2" >
        <xsl:value-of select="m:NoiseReduction"/>
      </td>
    </tr>
    <tr>
      <td class="label" >Digital process</td>
      <td class="value2" >
        <xsl:value-of select="m:DigitalProcess"/>
      </td>
    </tr>
    <!-- END of Audio Section ****************************** -->
  </xsl:template>

  <xsl:template name="feature">
    <!-- START of Feature Section ************************** -->
    <xsl:variable name="FeatureData">
      <xsl:if test="(./m:ClamShell) != 'false'">1</xsl:if>
      <xsl:if test="(./m:UneditedVersion) != 'false'">2</xsl:if>
      <xsl:if test="(./m:CollectorsEdition) != 'false'">3</xsl:if>
      <xsl:if test="(./m:DirectorsCut) != 'false'">4</xsl:if>
      <xsl:if test="(./m:ClosedCaptioned) != 'false'">5</xsl:if>
      <xsl:if test="(./m:Color) != ''">6</xsl:if>
      <xsl:if test="(./m:Letterboxed) != 'false'">7</xsl:if>
    </xsl:variable>
    <xsl:if test="$FeatureData != ''">
      <tr>
        <td class="label" >Features: </td>
        <td class="value2" >
          <xsl:if test="(./m:ClamShell) != 'false'">
            Clam Shell<br/>
          </xsl:if>
          <xsl:if test="(./m:UneditedVersion) != 'false'">
            Unedited Version<br/>
          </xsl:if>
          <xsl:if test="(./m:CollectorsEdition) != 'false'">
            Collectors Edition<br/>
          </xsl:if>
          <xsl:if test="(./m:DirectorsCut) != 'false'">
            Directors Cut<br/>
          </xsl:if>
          <xsl:if test="(./m:ClosedCaptioned) != 'false'">
            Closed Captioned<br/>
          </xsl:if>
          <xsl:if test="(./m:Color) != ''">
            <xsl:value-of select="./m:Color"/>
            <br/>
          </xsl:if>
          <xsl:if test="(./m:Letterboxed) != '(Unknown)' and (./m:Letterboxed) != 'false'">
            Letter Boxed<br/>
          </xsl:if>
          <!--
            <xsl:value-of select="$FeatureData"/>
-->
        </td>
      </tr>
    </xsl:if>
    <!-- END of Feature Section ************************** -->
  </xsl:template>

  <xsl:template name="rating">
    <!-- START of Rating Section ************************ -->
    <xsl:if test="(./m:RatingText) !=  ''">
      <tr>
        <td class="label" >Rating: </td>
        <td class="value2" >
          <xsl:value-of select="./m:RatingText"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(./m:RatingReason) !=  ''">
      <tr>
        <td class="label" >Rating Reason: </td>
        <td class="value2" >
          <xsl:value-of select="./m:RatingReason"/>
        </td>
      </tr>
    </xsl:if>
    <!-- END of Rating Section ************************ -->
  </xsl:template>

  <xsl:template name="extrainfo">
    <!-- START of ExtraInfo Section ************************ -->
    <xsl:if test="(./m:ExtraInfo) !=  ''">
      <tr>
        <td class="label" >Extra Info: </td>
        <td class="value2" >
          <xsl:value-of select="./m:ExtraInfo"/>
        </td>
      </tr>
    </xsl:if>
    <!-- END of ExtraInfo Section ************************ -->
  </xsl:template>

  <xsl:template name="category">
    <!-- START of Product Category Section ************************ -->
    <xsl:if test="count(./m:VideoProduct/m:ProdCategorys/m:ProdCategory) > 0">
      <tr>
        <td class="label">Category: </td>
        <td class="value3n">
          <xsl:for-each select="./m:VideoProduct/m:ProdCategorys/m:ProdCategory">
            <xsl:sort select="./m:CategoryLevel"/>
            <xsl:sort select="./m:CategoryName"/>

            <xsl:if test="(./m:CategoryLevel) = '1'">
              <span class="sep1B">
                <xsl:value-of select="./m:CategoryName"/>
              </span>
            </xsl:if>
            <xsl:if test="(./m:CategoryLevel) != '1'">
              <span class="sep1">
                <xsl:value-of select="./m:CategoryName"/>
              </span>
            </xsl:if>
            <xsl:if test = "not(position()=last())">
              <span class="sep2">
                <xsl:text>; </xsl:text>
              </span>
            </xsl:if>
          </xsl:for-each>
        </td>
      </tr>
    </xsl:if>
    <!-- END of Product Category Section ************************ -->
  </xsl:template>

  <xsl:template name="silent">
    <!-- START of Silent Section ************************ -->
    <xsl:if test="(m:VideoProduct/m:Silent) != 'false'">
      <tr>
        <td class="label" >Silent: </td>
        <td class="value2" >
          <xsl:value-of select="m:VideoProduct/m:Silent"/>
        </td>
      </tr>
    </xsl:if>
    <!-- END of Silent Section ************************ -->
  </xsl:template>

  <xsl:template name="origrelease">
    <!-- START of OrigReleaseYear Section ************************ -->
    <xsl:if test="(m:VideoProduct/m:OrigReleaseYear) != ''">
      <tr>
        <td class="label" >Orig Release Year: </td>
        <td class="value2" >
          <xsl:value-of select="m:VideoProduct/m:OrigReleaseYear"/>
        </td>
      </tr>
    </xsl:if>
    <!-- END of OrigReleaseYear Section ************************ -->
  </xsl:template>

  <xsl:template name="releasecompany">
    <!-- START of ReleaseCompanyName Section ************************ -->
    <xsl:if test="(m:ReleaseCompanyName) != ''">
      <tr>
        <td class="label" >Released by: </td>
        <td class="value2" >
          <xsl:value-of select="m:ReleaseCompanyName"/>
        </td>
      </tr>
    </xsl:if>
    <!-- END of ReleaseCompanyName Section ************************ -->
  </xsl:template>

  <xsl:template name="catalog">
    <!-- START of CatalogNumber Section ************************ -->
    <xsl:if test="(m:CatalogNumber) != ''">
      <tr>
        <td class="label" >Catalog Number: </td>
        <td class="value2" >
          <xsl:value-of select="m:CatalogNumber"/>
        </td>
      </tr>
    </xsl:if>
    <!-- END of CatalogNumber Section ************************ -->
  </xsl:template>

  <xsl:template name="area">
    <!-- START of Area Section ************************ -->
    <xsl:if test="(m:VideoProduct/m:Area) != ''">
      <tr>
        <td class="label" >Area: </td>
        <td class="value2" >
          <xsl:value-of select="m:VideoProduct/m:Area"/>
        </td>
      </tr>
    </xsl:if>
  </xsl:template>

  <xsl:template name="award">
    <!-- *********************** START of Awards Section ***************************** -->
    <xsl:for-each select="./m:VideoProduct/m:Awards/m:Award">
      <xsl:if test="(.) != ''">
        <tr>
          <td class="label" >
            <xsl:value-of select="m:AwardGivrName"/>
            <xsl:text>: </xsl:text>
          </td>
          <td class="value2" >
            <xsl:for-each select=".">
              <xsl:value-of select="./m:AwardCatName"/>
              <xsl:text> - </xsl:text>
              <xsl:value-of select="./m:AwardStatus"/>
              <xsl:text>: </xsl:text>
              <xsl:value-of select="./m:AwardYear"/>
              <xsl:text></xsl:text>
              <br/>
              <xsl:for-each select="./m:Contributor">
                <xsl:if test="(m:ContribAkType) = 'Primary Name'">
                  <span class="Muzebold">
                    <!-- removed for ORS 1/17/2011  RCW
                                        <a class="Muze" style="font-weight:bold; color:#315E74;" href="#Top">
-->
                    <xsl:if test="(m:FirstName) !=''">
                      <xsl:value-of select="./m:FirstName"/>
                    </xsl:if>
                    <xsl:if test="(m:LastName) !='Not Applicable'">
                      <xsl:text > 
                                        </xsl:text>
                      <xsl:value-of select="m:LastName"/>
                    </xsl:if>
                    <!-- removed for ORS 1/17/2011  RCW
                                        </a>
-->
                  </span>
                </xsl:if>
                <xsl:if test="(m:ContribAkType) != 'Primary Name'">
                  <xsl:if test="(m:FirstName) !=''">
                    <xsl:value-of select="./m:FirstName"/>
                  </xsl:if>
                  <xsl:if test="(m:LastName) !='Not Applicable'">
                    <xsl:text > </xsl:text>
                    <xsl:value-of select="m:LastName"/>
                  </xsl:if>
                </xsl:if>
                <!--                            <xsl:if test="(./m:ShortDescription) !=''"><xsl:text > </xsl:text><span class="gray">
                            <xsl:value-of select="./m:ShortDescription"/></span></xsl:if>
-->
                <xsl:text > </xsl:text>
                <span class="gray">
                  <xsl:value-of select="./m:ShortDescription"/>
                </span>
              </xsl:for-each>
            </xsl:for-each>
          </td>
        </tr>
      </xsl:if>
    </xsl:for-each>
    <!-- *********************** END of Awards Section *************************** -->
    <!-- START of ReleasePrices Section  -->
    <!--    <xsl:if test="count(./PreBook) >  0"><br/><xsl:value-of select="./PreBook"/> - PreBook Date</xsl:if>  -->
    <!-- ************************************************************************* -->
  </xsl:template>

  <xsl:template name="contributor">
    <xsl:variable name="Acomma">%</xsl:variable>
    <!-- *********************** START of ProdContribJobs Section ******************** -->
    <xsl:for-each select="./m:VideoProduct/m:ProdContribJobs/m:ProdContribJob">
      <xsl:sort select="./m:JobName"/>
      <tr>
        <td class="label" >
          <xsl:value-of select="./m:JobName"/>:
        </td>
        <td class="value2" >
          <xsl:if test="count(./m:Contributor) > 0">
            <xsl:for-each select="./m:Contributor">
              <xsl:if test="(../m:PrimaryContrib) = 'true'">
                <xsl:variable name="PrimaryArtist">
                  <xsl:text>srchd.jsp?hlk=art1:</xsl:text>
                  <xsl:if test="(./m:LastName) !=''">
                    <xsl:value-of select="./m:LastName"/>
                  </xsl:if>
                  <xsl:if test="(./m:FirstName) !=''">
                    <xsl:value-of select="$Acomma" />
                    <xsl:text>2c+</xsl:text>
                    <xsl:value-of select="./m:FirstName"/>
                  </xsl:if>
                  <xsl:if test="(./m:Prefix) !=''">
                    <xsl:value-of select="$Acomma" />
                    <xsl:text>2c+</xsl:text>
                    <xsl:value-of select="./m:Prefix"/>
                  </xsl:if>
                  <xsl:text></xsl:text>
                  <xsl:if test="(./m:Suffix) !=''">
                    <xsl:value-of select="$Acomma" />
                    <xsl:text>2c+</xsl:text>
                    <xsl:value-of select="./m:Suffix"/>
                  </xsl:if>
                  <xsl:text></xsl:text>
                </xsl:variable>
                <!-- removed link 1/17/2011 for ORS by RCW
                            <span class="Muzebold"><a class="Muze" style="font-weight:bold; color:#315E74;" href="{$PrimaryArtist}">
                            <xsl:if test="(./m:Prefix) !=''"><xsl:value-of select="./m:Prefix"/><xsl:text > </xsl:text> </xsl:if>
                            <xsl:if test="(./m:FirstName) !=''"><xsl:value-of select="./m:FirstName"/> </xsl:if>
                            <xsl:if test="(./m:LastName) !=''"><xsl:text > 
                            </xsl:text><xsl:value-of select="./m:LastName"/></xsl:if>
                            <xsl:if test="(./m:Suffix) !=''"><xsl:text > </xsl:text></xsl:if><xsl:value-of select="./Suffix"/></a></span>
-->
                <span class="Muzebold">
                  <xsl:if test="(./m:Prefix) !=''">
                    <xsl:value-of select="./m:Prefix"/>
                    <xsl:text > </xsl:text>
                  </xsl:if>
                  <xsl:if test="(./m:FirstName) !=''">
                    <xsl:value-of select="./m:FirstName"/>
                  </xsl:if>
                  <xsl:if test="(./m:LastName) !=''">
                    <xsl:text > 
                            </xsl:text>
                    <xsl:value-of select="./m:LastName"/>
                  </xsl:if>
                  <xsl:if test="(./m:Suffix) !=''">
                    <xsl:text > </xsl:text>
                  </xsl:if>
                  <xsl:value-of select="./Suffix"/>
                </span>
                <!-- end of substituted code 1/17/2011 RCW  -->
              </xsl:if>
              <xsl:if test="(../m:PrimaryContrib) != 'true'">
                <xsl:if test="(./m:FirstName) !=''">
                  <xsl:value-of select="./m:FirstName"/>
                </xsl:if>
                <xsl:if test="(./m:LastName) !=''">
                  <xsl:text > </xsl:text>
                  <xsl:value-of select="./m:LastName"/>
                </xsl:if>
              </xsl:if>
              <xsl:if test="(./m:ShortDescription) !=''">
                <xsl:text > </xsl:text>
                <span class="gray">
                  <xsl:value-of select="./m:ShortDescription"/>
                </span>
              </xsl:if>
            </xsl:for-each>
          </xsl:if>
        </td>
      </tr>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="synopsis">
    <!-- ******************* START of NoteForProduct Section case 1 ****************** -->
    <xsl:for-each select="./m:VideoProduct/m:NoteForProducts/m:NoteForProduct">
      <xsl:if test="(./m:NoteTypeDescr) = 'Muze Description' or (./m:NoteTypeDescr) = 'Title Note'">
        <tr>
          <td class="label" >
            <xsl:if test="(./m:NoteTypeDescr) = 'Muze Description'">
              <xsl:text>Synopsis: </xsl:text>
            </xsl:if>
            <xsl:if test="(./m:NoteTypeDescr) = 'Title Note'">
              <xsl:value-of select="./m:NoteTypeDescr"/>:
            </xsl:if>
          </td>
          <td class="value2" >
            <xsl:if test="(./m:NoteText) !=''">
              <xsl:value-of select="./m:NoteText"/>
            </xsl:if>
          </td>
        </tr>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="releasenote">
    <!-- ******************* START of NoteReleaseItem Section ************************ -->
    <!--    <xsl:for-each select="./m:NoteReleaseItems/m:NoteReleaseItem"> -->
    <!--        <xsl:if test="(./m:NoteTypeDescr) != '' and (./m:NoteText) != ''"> -->
    <!--            <tr> -->
    <!--                <td class="label" ><xsl:value-of select="./m:NoteTypeDescr"/></td> -->
    <!--                <td class="value2" ><xsl:value-of select="./m:NoteText"/></td> -->
    <!--            </tr> -->
    <!--        </xsl:if> -->
    <!--    </xsl:for-each> -->
    <!--</xsl:template> -->
    <xsl:for-each select="./m:NoteReleaseItems/m:NoteReleaseItem">
      <xsl:if test="(./m:NoteTypeDescr) != '' and (./m:NoteText) != ''">
        <xsl:variable name="StringIn">
          <xsl:value-of select="./m:NoteText"/>
        </xsl:variable>
        <xsl:variable name="ResultTreeFragmentOut">
          <xsl:call-template name="lf2br">
            <xsl:with-param name="StringToTransform" select="$StringIn"/>
          </xsl:call-template>
        </xsl:variable>
        <tr>
          <td class="label" >
            <xsl:value-of select="./m:NoteTypeDescr"/>
          </td>
          <td class="value2" >
            <xsl:copy-of select="$ResultTreeFragmentOut"/>
          </td>
        </tr>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="reviews">
    <!-- ******************* START of NoteForProduct Section case 2 ****************** -->
    <xsl:for-each select="./m:VideoProduct/m:NoteForProducts/m:NoteForProduct">
      <xsl:if test="(./m:NoteTypeDescr) != 'Muze Description' and (./m:NoteTypeDescr) != 'Title Note'">
        <tr>
          <td class="label" >
            <xsl:value-of select="./m:NoteTypeDescr"/>:
          </td>
          <td class="value2" >
            <span class="Muzebold">
              <xsl:value-of select="./m:SourceName"/>
            </span>
            <xsl:if test="(./m:SourceDateString) !=''">
              - <xsl:value-of select="./m:SourceDateString"/>
            </xsl:if>
            <xsl:if test="(./m:SourceVolume) !=''">
              ; <xsl:value-of select="./m:SourceVolume"/>
            </xsl:if>
            <xsl:if test="(./m:NoteText) !=''">
              <br/>
              <xsl:value-of select="./m:NoteText"/>
            </xsl:if>
            <!--            		<xsl:if test="count(./m:VideoProduct/m:NoteForProducts/m:NoteForProduct/m:Contributor) >  0">
-->
            <xsl:for-each select="./m:Contributor">
              <br/>
              <xsl:text> - </xsl:text>
              <xsl:if test="(./m:FirstName) !=''">
                <xsl:value-of select="./m:FirstName"/>
              </xsl:if>
              <xsl:text> </xsl:text>
              <xsl:if test="(./m:LastName) !=''">
                <xsl:value-of select="./m:LastName"/>
              </xsl:if>
              <xsl:if test="(./m:ShortDescription) !=''">
                - <span class="gray">
                  <xsl:value-of select="./m:ShortDescription"/>
                </span>
              </xsl:if>
            </xsl:for-each>
            <!--</xsl:if>-->
          </td>
        </tr>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="allsimilar">
    <h2>Similar Video Releases</h2>
    <table class="MuzeRight" style="vertical-align:top; ">
      <tr>
        <td colspan="2" class="similar">
          <xsl:call-template name="allUPC"/>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template name="similartitle">
    <h2>Similar Video Releases - Unique Titles</h2>
    <table class="MuzeRight" style="vertical-align:top; ">
      <xsl:call-template name="alltitleUPC"/>
    </table>
  </xsl:template>

  <xsl:template name="allUPC">
    <!-- The following captures all the UPCs and DOES NOT end with semi-colon -->
    <xsl:variable name="RelatedUPCss">
      <xsl:for-each select="../m:SimilarCinema[position()=1]/m:SimilarCinemaRelease/m:UPC">
        <xsl:value-of select="."/>
        <xsl:text>;</xsl:text>
      </xsl:for-each>
    </xsl:variable>
    <!-- ***************************************************************************** -->
    <!-- The form mechanism was created and is maintained by leadSOFT -->
    <a href="javascript:LinkMuzeUPC('muze_l0')">View ALL Similar Releases</a>
    <input type='hidden' name='muze_l0' value='{$RelatedUPCss}' />
    <!-- ***************************************************************************** -->
  </xsl:template>

  <xsl:template name="alltitleUPC">
    <!-- This collects all UPCs for each unique Title and builds forms for leadSOFT javascript -->
    <xsl:for-each select="../m:SimilarCinema[position()=1]/m:SimilarCinemaRelease/m:Title[not(.=preceding::m:Title)]">
      <xsl:sort select="."/>
      <xsl:variable name="indexPos" select="position()"/>
      <tr>
        <td colspan="2" class="similar">
          <xsl:variable name="RelatedUPC">
            <xsl:value-of select="."/>
          </xsl:variable>
          <!--                    <xsl:value-of select="."/>
                <xsl:value-of select="../m:UPC"/>
                <xsl:value-of select="$RelatedUPC"/>
-->
          <!-- The following captures all title related UPCs  -->
          <xsl:variable name="RelatedUPCs">
            <xsl:for-each select="//m:Muze/m:SimilarCinema[position()=1]/m:SimilarCinemaRelease/m:Title">
              <xsl:if test="(.) =$RelatedUPC">
                <xsl:value-of select="../m:UPC"/>
                <xsl:text>;</xsl:text>
              </xsl:if>
            </xsl:for-each>
          </xsl:variable>
          <!-- ***************************************************************************** -->
          <!-- The form mechanism was created and is maintained by leadSOFT 
                <xsl:if test="(../m:TitleArticle) != ''"><xsl:value-of select="../m:TitleArticle"/><xsl:text> </xsl:text></xsl:if><xsl:value-of select="."/>
    -->
          <!--  Original Code removed 1/17/2010 for ORS to hide TS3 search links  (RCW)-->
          <a class="Muze" title="{.}" href="javascript:BTNextGenJS.LinkMuzeUPC('muze_l{$indexPos}')">
            <xsl:if test="(../m:TitleArticle) != ''">
              <xsl:value-of select="../m:TitleArticle"/>
              <xsl:text> </xsl:text>
            </xsl:if>
            <xsl:value-of select="."/>
          </a>
          <input type='hidden' name='muze_l{$indexPos}' value='{$RelatedUPCs}'/>

          <!-- ***************************************************************************** -->
        </td>
      </tr>
    </xsl:for-each>
  </xsl:template>




  <!-- *************** SECTION TWO is Popular Music ************************************ -->
  <xsl:template name="PopMusic" match="//m:PopularMusic">
    <!-- START: PopularMusic Content ***************************************************** -->
    <!-- <xsl:if test="count(//PopularMusic) > '0' or count(//Songs) > '0' or count(//EssentialArtist) > '0'"> -->
  <div>
        <img src="/_layouts/IMAGES/CSDefaultSite/assets/images/Rovi_Logo_Hero_Black_RGB.JPG" style="height:40px;width:66px" />
        <p class="rovitext">
          Portions of Content Provided by Rovi Corporation. &copy;&nbsp;[copyrightyear] Rovi Corporation
        </p>
  </div>
    <br/>
    <table class="Muze" style="width:600px !important;">
      <tr>
        <!-- *********** START of MuzeLeft COLUMN **************************************** -->
        <td class="MuzeLeft">
          <!-- ***************************************************************************** -->
          <!-- *********************** START of Popular Music Table ************************ -->
          <!-- <xsl:if test="count(//PopularMusic) >  '0'"> -->
          <xsl:if test="$DoPopular = '1'">
            <xsl:call-template name="popdetail"/>
          </xsl:if>


          <!-- *********************** END of Popular Music Table ************************** -->


          <!-- ************ START of Popular Music PREVIEWS PNOTES Table ******************* -->
          <xsl:if test="(./m:PREVIEWS) or (./m:PNOTES)">
            <xsl:call-template name="popreviews"/>
          </xsl:if>
          <!-- ************ END of Popular Music PREVIEWS PNOTES Table *********************-->


          <!-- **************** START of Essential Artists Table *************************** -->
          <xsl:if test="(../m:EssentialArtist)">
            <h2>Essential Artists</h2>
            <table class="MuzeLeft">
              <xsl:call-template name="essential"/>
            </table>
          </xsl:if>
          <!-- ****************** End of Essential Artist Table **************************** -->
        </td>
        <!-- *********** END of MuzeLeft COLUMN ****************************************** -->
        <!-- *********** START of MuzeRight COLUMN *************************************** -->
        <td class="MuzeRight">
          <!-- *********** START of Popular Music Track Listing Table ********************** -->
          <!--     <xsl:if test="count(//Songs) >  0"> -->
          <xsl:if test="$DoSongs = '1'">
            <xsl:call-template name="popsongs"/>
          </xsl:if>
          <!-- *********** END of Popular Music Track Listing Table ************************ -->
        </td>
        <!-- ************ END of MuzeRight COLUMN **************************************** -->
      </tr>
    </table>
    <!-- </xsl:if> -->
    <!-- END: PopularMusic Content ******************************************************* -->
  </xsl:template>

  <xsl:template name="popdetail">
    <h2>Detailed Product Description: Popular Music</h2>
    <table  class="MuzeLeft">
      <xsl:call-template name="titleperform"/>
      <xsl:call-template name="release"/>
      <xsl:call-template name="genre"/>
      <xsl:call-template name="guests"/>
      <xsl:call-template name="produce"/>
      <xsl:call-template name="distribute"/>
      <xsl:call-template name="recording"/>
      <xsl:call-template name="studiolive"/>
      <xsl:call-template name="popSPAR"/>
      <xsl:call-template name="poporigin"/>
    </table>
  </xsl:template>

  <xsl:template name="titleperform">
    <!-- ************************ START of TITLE Section ************************-->
    <xsl:if test="(./m:TITLE) != ''">
      <tr>
        <td class="label">Title: </td>
        <td class="value2">
          <xsl:value-of select="./m:TITLE"/>
        </td>
      </tr>
    </xsl:if>
    <!-- ************************ START of TITLE Section ************************-->
    <!-- ********************* START of PERFORMER Section ***********************-->
    <xsl:if test="(./m:PERFORMER) != ''">
      <tr>
        <td class="label">Performer: </td>
        <td class="value2">
          <xsl:value-of select="./m:PERFORMER"/>
        </td>
      </tr>
    </xsl:if>
    <!-- ********************* END of PERFORMER Section ***********************-->
  </xsl:template>

  <xsl:template name="release">
    <!-- ********************* START of LABELNAME Section ***********************-->
    <xsl:if test="(./m:LABELNAME) != ''">
      <tr>
        <td class="label">Record Label: </td>
        <td class="value2">
          <xsl:value-of select="./m:LABELNAME"/>
        </td>
      </tr>
    </xsl:if>
    <!-- ********************* END of LABELNAME Section ***********************-->
    <!-- ********************* START of RELEASED Section ***********************-->
    <!--
    <xsl:for-each select="./RELEASED">
-->
    <xsl:if test="(./m:RELEASED) != ''">
      <tr>
        <td class="label">Release Date: </td>
        <td class="value2">
          <xsl:value-of select="./m:RELEASED"/>
        </td>
      </tr>
    </xsl:if>
    <!--
    </xsl:for-each>
-->
    <!-- ********************* END of RELEASED Section ***********************-->
    <!-- ********************** START of ORIGREL Section ***********************-->
    <xsl:if test="(./m:ORIGREL) != ''">
      <tr>
        <td class="label">Original Release: </td>
        <td class="value2">
          <xsl:value-of select="./m:ORIGREL"/>
        </td>
      </tr>
    </xsl:if>
    <!-- ********************** END of ORIGREL Section ***********************-->
  </xsl:template>

  <xsl:template name="genre">
    <!-- ********************** START of CAT3:CAT4 Section *********************-->
    <!-- ********************** START of CAT2 Section **************************-->
    <xsl:if test="(./m:CAT2) != ''">
      <tr>
        <td class="label">General Description: </td>
        <td class="value2">
          <xsl:value-of select="./m:CAT2"/>
        </td>
      </tr>
    </xsl:if>
    <!-- ********************** END of CAT2 Section **************************-->
    <xsl:if test="(./m:CAT3) != ''">
      <tr>
        <td class="label">Muze Genre - Sub Class: </td>
        <td class="value2">
          <xsl:value-of select="./m:CAT3"/>

          <xsl:if test="(./m:CAT4) != ''">
            <xsl:text> - </xsl:text>
            <xsl:value-of select="./m:CAT4"/>
          </xsl:if>
        </td>
      </tr>
    </xsl:if>
    <!-- ********************** END of CAT3:CAT4 Section *********************-->
  </xsl:template>

  <xsl:template name="guests">
    <!-- ************************ START of ARTIST1 Section ********************* -->
    <!--<xsl:for-each select="//ARTIST1">-->
    <xsl:if test="(./m:ARTIST1) != ''">
      <tr>
        <td class="label">Guest Artists: </td>
        <td class="value2">
          <xsl:value-of select="./m:ARTIST1"/>
        </td>
      </tr>
    </xsl:if>
    <!--</xsl:for-each>-->
    <!-- ************************ END of ARTIST1 Section ********************* -->
  </xsl:template>

  <xsl:template name="produce">
    <!-- ************************ START of ENGINEER Section *********************-->
    <xsl:if test="(./m:ENGINEER) != ''">
      <tr>
        <td class="label">Engineer: </td>
        <td class="value2">
          <xsl:value-of select="./m:ENGINEER"/>
        </td>
      </tr>
    </xsl:if>
    <!-- ************************ END of ENGINEER Section *********************-->
    <!-- *********************** START of PRODUCER Section ********************* -->
    <xsl:if test="(./m:PRODUCER) != ''">
      <tr>
        <td class="label">Produced by: </td>
        <td class="value2">
          <xsl:value-of select="./m:PRODUCER"/>
        </td>
      </tr>
    </xsl:if>
    <!-- *********************** END of PRODUCER Section ********************* -->
  </xsl:template>

  <xsl:template name="distribute">
    <!-- ******************** START of DISTRIBUT Section *********************** -->
    <xsl:for-each select="./m:DISTRIBUT">
      <xsl:if test="(./m:DISTRIBUT) != ''">
        <tr>
          <td class="label">US Distributor: </td>
          <td class="value2">
            <xsl:value-of select="./m:DISTRIBUT"/>
          </td>
        </tr>
      </xsl:if>
    </xsl:for-each>
    <!-- ******************** END of DISTRIBUT Section *********************** -->
    <!-- ******************** START of CATALOG Section ************************** -->
    <xsl:if test="(./m:CATALOG) != ''">
      <tr>
        <td class="label">Catalog Number: </td>
        <td class="value2">
          <xsl:value-of select="./m:CATALOG"/>
        </td>
      </tr>
    </xsl:if>
    <!-- ******************** END of CATALOG Section ************************** -->
  </xsl:template>

  <xsl:template name="recording">
    <!-- ******************** START of NBRDISCS Section ************************ -->
    <xsl:if test="(./m:NBRDISCS) != ''">
      <tr>
        <td class="label">Number of Discs: </td>
        <td class="value2">
          <xsl:value-of select="./m:NBRDISCS"/>
        </td>
      </tr>
    </xsl:if>
    <!-- ******************** END of NBRDISCS Section ************************ -->
    <!-- ******************** START of Running Time Section ******************** -->
    <xsl:if test="(./m:MINUTES) != '' or (./m:SECONDS) != ''">
      <tr>
        <td class="label">Running Time: </td>
        <td class="value2">
          <xsl:if test="(./m:MINUTES) != ''">
            <xsl:value-of select="./m:MINUTES"/>
            <xsl:text> Minutes </xsl:text>
          </xsl:if>
          <xsl:if test="(./m:SECONDS) != ''">
            <xsl:value-of select="./m:SECONDS"/>
            <xsl:text> Seconds</xsl:text>
          </xsl:if>
        </td>
      </tr>
    </xsl:if>
    <!-- ******************** END of Running Time Section ******************** -->
    <!-- ******************** START of MONOSTEREO Section ********************** -->
    <xsl:if test="(./m:MONOSTEREO) != ''">
      <tr>
        <td class="label">Stereo/Mono: </td>
        <td class="value2">
          <xsl:value-of select="./m:MONOSTEREO"/>
        </td>
      </tr>
    </xsl:if>
    <!-- ******************** END of MONOSTEREO Section ********************** -->
  </xsl:template>

  <xsl:template name="studiolive">
    <!-- ******************** START of STUDIOLIVE Section ********************** -->
    <xsl:if test="(./m:STUDIOLIVE) != ''">
      <tr>
        <td class="label">Performance Recorded: </td>
        <xsl:if test="(./m:STUDIOLIVE) = 'Live'">
          <td class="value2">
            <xsl:text>Live Performance</xsl:text>
          </td>
        </xsl:if>
        <xsl:if test="(./m:STUDIOLIVE) = 'Studio'">
          <td class="value2">
            <xsl:text>Studio Recording</xsl:text>
          </td>
        </xsl:if>
        <xsl:if test="(./m:STUDIOLIVE) = 'Mixed'">
          <td class="value2">
            <xsl:text>Mix of Live Performance and Studio Recordings</xsl:text>
          </td>
        </xsl:if>
      </tr>
    </xsl:if>
    <!-- ******************** END of STUDIOLIVE Section ********************** -->
  </xsl:template>

  <xsl:template name="popSPAR">
    <!-- ************************** START of SPAR Section ********************** -->
    <xsl:if test="(./m:SPAR) != 'n/a'">
      <tr>
        <td class="label">SPAR Recording Mix: </td>
        <td class="value2">
          <xsl:choose>
            <xsl:when test="(./m:SPAR) = 'AAA'">
              <xsl:value-of select="./m:SPAR"/>
              <xsl:text>: Analog Recording; Analog Mixing; Analog Mastering</xsl:text>
            </xsl:when>
            <xsl:when test="(./m:SPAR) = 'AAD'">
              <xsl:value-of select="./m:SPAR"/>
              <xsl:text>: Analog Recording; Analog Mixing; Digital Mastering</xsl:text>
            </xsl:when>
            <xsl:when test="(./m:SPAR) = 'ADA'">
              <xsl:value-of select="./m:SPAR"/>
              <xsl:text>: Analog Recording; Digital Mixing; Analog Mastering</xsl:text>
            </xsl:when>
            <xsl:when test="(./m:SPAR) = 'ADD'">
              <xsl:value-of select="./m:SPAR"/>
              <xsl:text>: Analog Recording; Digital Mixing; Digital Mastering</xsl:text>
            </xsl:when>
            <xsl:when test="(./m:SPAR) = 'DAA'">
              <xsl:value-of select="./m:SPAR"/>
              <xsl:text>: Digital Recording; Analog Mixing; Analog Mastering</xsl:text>
            </xsl:when>
            <xsl:when test="(./m:SPAR) = 'DAD'">
              <xsl:value-of select="./m:SPAR"/>
              <xsl:text>: Digital Recording; Analog Mixing; Digital Mastering</xsl:text>
            </xsl:when>
            <xsl:when test="(./m:SPAR) = 'DDA'">
              <xsl:value-of select="./m:SPAR"/>
              <xsl:text>: Digital Recording; Digital Mixing; Analog Mastering</xsl:text>
            </xsl:when>
            <xsl:when test="(./m:SPAR) = 'DDD'">
              <xsl:value-of select="./m:SPAR"/>
              <xsl:text>: Digital Recording; Digital Mixing; Digital Mastering</xsl:text>
            </xsl:when>
          </xsl:choose>
        </td>
      </tr>
    </xsl:if>
    <!-- ************************** END of SPAR Section ********************** -->
  </xsl:template>

  <xsl:template name="poporigin">
    <!-- *********************** START of IMPORT Section *********************** -->
    <xsl:if test="(./m:IMPORT) != '' or (./m:AREA) != ''">
      <tr>
        <td class="label">Country of Origin: </td>
        <td class="value2">
          <xsl:if test="(./m:IMPORT) = 'Y'">
            <xsl:text>Import</xsl:text>
            <xsl:if test="(./m:AREA) != ''">
              <xsl:text> from </xsl:text>
              <xsl:value-of select="./m:AREA"/>
            </xsl:if>
          </xsl:if>

          <xsl:if test="(./m:IMPORT) = 'N'">
            <xsl:text>Domestic</xsl:text>
            <xsl:if test="(./m:AREA) != ''">
              <xsl:text> to </xsl:text>
              <xsl:value-of select="./m:AREA"/>
            </xsl:if>
          </xsl:if>
        </td>
      </tr>
    </xsl:if>
    <!-- *********************** END of IMPORT Section *********************** -->
  </xsl:template>

  <xsl:template name="popreviews">
    <h2>Commentary and Reviews</h2>
    <table class="MuzeLeft">
      <!-- ******************** START of PREVIEWS Section ************************ -->
      <xsl:for-each select="./m:PREVIEWS">
        <xsl:if test="(.) != ''">
          <tr>
            <td class="label">Review Excerpts: </td>
            <td class="value2">
              <xsl:value-of select="."/>
            </td>
          </tr>
        </xsl:if>
      </xsl:for-each>
      <!-- ******************** END of PREVIEWS Section ************************ -->
      <!-- ******************** START of PNOTES Section ************************** -->
      <xsl:for-each select="./m:PNOTES">
        <xsl:if test="(.) != ''">
          <tr>
            <td class="label">Misc. Notes: </td>
            <td class="value2">
              <xsl:value-of select="."/>
            </td>
          </tr>
        </xsl:if>
      </xsl:for-each>
      <!-- ******************** END of PNOTES Section ************************** -->
    </table>
  </xsl:template>

  <xsl:template name="essential">
    <!-- ******************** START of Performer Section *********************** -->
    <xsl:for-each select="../m:EssentialArtist/m:Performer">
      <xsl:if test="(.) != ''">
        <tr>
          <td class="label" >Performer: </td>
          <td class="value2" >
            <xsl:value-of select="."/>
          </td>
        </tr>
      </xsl:if>
    </xsl:for-each>
    <!-- ******************** END of Performer Section *********************** -->
    <!-- ******************** START of Overview  Section ************************ -->
    <xsl:for-each select="../m:EssentialArtist/m:Overview">
      <xsl:if test="(.) != ''">
        <tr>
          <td class="label" >Overview of Performer: </td>
          <td class="value2" >
            <xsl:value-of select="."/>
          </td>
        </tr>
      </xsl:if>
    </xsl:for-each>
    <!-- ******************** END of Overview  Section ************************ -->
    <!-- ******************** START of the Influences Section ************************ -->
    <xsl:if test = "(../m:EssentialArtist/m:Influences/m:Performer)">
      <tr>
        <td class="label" >Performer Influenced by: </td>
        <td class="value2" >
          <xsl:for-each select="../m:EssentialArtist/m:Influences/m:Performer">
            <xsl:value-of select="."/>
            <xsl:if test = "position() != 'last'">
              <br />
            </xsl:if>
          </xsl:for-each>
        </td>
      </tr>
    </xsl:if>
    <!-- ******************** END of the Influences Section ************************ -->
    <!-- ******************** START of the Followers Section ************************ -->
    <xsl:if test = "(../m:EssentialArtist/m:Followers/m:Performer)">
      <tr>
        <td class="label" >Influenced by Performer: </td>
        <td class="value2" >
          <xsl:for-each select="../m:EssentialArtist/m:Followers/m:Performer">
            <xsl:value-of select="."/>
            <xsl:if test = "position() != 'last'">
              <br />
            </xsl:if>
          </xsl:for-each>
        </td>
      </tr>
    </xsl:if>
    <!-- ******************** END of the Followers Section ************************ -->
    <!-- ******************** START of the Contemporaries Section ************************ -->
    <xsl:if test = "(../m:EssentialArtist/m:Contemporaries/m:Performer)">
      <tr>
        <td class="label" >Contemporaries of Performer: </td>
        <td class="value2" >
          <xsl:for-each select="../m:EssentialArtist/m:Contemporaries/m:Performer">
            <xsl:value-of select="."/>
            <xsl:if test = "position() != 'last'">
              <br />
            </xsl:if>
          </xsl:for-each>
        </td>
      </tr>
    </xsl:if>
    <!-- ******************** END of the Contemporaries Section ************************ -->
    <!-- ******************** START of the Definitive Albums Section ************************ -->
    <xsl:if test = "(../m:EssentialArtist/m:DefinitiveAlbums/m:Album)">
      <tr>
        <td class="label" >Definitive Albums: </td>
        <td class="value2" >
          <xsl:for-each select="../m:EssentialArtist/m:DefinitiveAlbums/m:Album">
            <xsl:value-of select="."/>
            <xsl:if test = "position() != 'last'">
              <br />
            </xsl:if>
          </xsl:for-each>
        </td>
      </tr>
    </xsl:if>
    <!-- ******************** END of the Definitive Albums Section ************************ -->
    <!-- ******************** START of the Recommended Videos Section ************************ -->
    <xsl:if test = "(../m:EssentialArtist/m:RecommendedVideos/m:Video)">
      <tr>
        <td class="label" >Recommended Videos: </td>
        <td class="value2" >
          <xsl:for-each select="../m:EssentialArtist/m:RecommendedVideos/m:Video">
            <xsl:value-of select="."/>
            <xsl:if test = "position() != 'last'">
              <br />
            </xsl:if>
          </xsl:for-each>
        </td>
      </tr>
    </xsl:if>
    <!-- ******************** END of the Recommended Videos Section ************************ -->
    <!-- ******************** START of the Recommended Books Section ************************ -->
    <xsl:if test = "(../m:EssentialArtist/m:RecommendedBooks/m:Book)">
      <tr>
        <td class="label" >Recommended Books: </td>
        <td class="value2" >
          <xsl:for-each select="../m:EssentialArtist/m:RecommendedBooks/m:Book">
            <xsl:value-of select="."/>
            <xsl:if test = "position() != 'last'">
              <br />
            </xsl:if>
          </xsl:for-each>
        </td>
      </tr>
    </xsl:if>
    <!-- ******************** END of the Recommended Books Section ************************ -->
  </xsl:template>

  <xsl:template name="popsongs">
    <h2>
      Track Listing<xsl:if test="(./m:NBRDISCS) != '1' and (./m:NBRDISCS) != '0'">
        <xsl:text>s</xsl:text>
      </xsl:if>
    </h2>
    <xsl:for-each select="./m:Songs">
      <table class="MuzeRight" style="vertical-align:top; ">
        <th>Track</th>
        <th>Song</th>
        <xsl:for-each select="//m:PopularMusic/m:Songs/m:Song">
          <xsl:sort select="./m:DISC" data-type="number"/>
          <xsl:sort select="./m:TRK" data-type="number"/>
          <xsl:if test="(./m:TRK) = '0'">
            <tr>
              <td colspan="2" class="Label" style="text-decoration:underline; padding-top:10px;">
                <xsl:value-of select="./m:SONG"/>
              </td>
            </tr>
          </xsl:if>
          <xsl:if test="(./m:TRK) != '0'">
            <tr>
              <td class="Track">
                <xsl:value-of select="./m:TRK"/>
              </td>
              <td class="Song">
                <xsl:value-of select="./m:SONG"/>
              </td>
            </tr>
          </xsl:if>
        </xsl:for-each>
      </table>
    </xsl:for-each>
  </xsl:template>

  <!-- *************** SECTION THREE is ClassicalMusic ********************************* -->
  <xsl:template name="ClassMusic" match="//m:ClassicalMusic">
    <!-- START: ClassicalMusic Content *********************************************** -->
    <!-- <xsl:if test="count(//ClassicalMusic) > '0'"> -->
  <div>
        <img src="/_layouts/IMAGES/CSDefaultSite/assets/images/Rovi_Logo_Hero_Black_RGB.JPG" style="height:40px;width:66px" />
        <p class="rovitext">
          Portions of Content Provided by Rovi Corporation. &copy;&nbsp;[copyrightyear] Rovi Corporation
        </p>
  </div>
    <br/>
    <table class="Muze" style="width:600px !important;">
      <tr>
        <!-- *********** START of MuzeLeft COLUMN ************************************ -->
        <td class="MuzeLeft">
          <!-- *********************** START of Classical Music Table ************** -->
          <!--     <xsl:if test="count(//ClassicalMusic) >  '0'">  -->
          <xsl:if test="$DoClassical = '1'">
            <xsl:call-template name="classdetail"/>
          </xsl:if>
          <!-- ************ START of Classical Music PREVIEWS TABLE **************** -->
          <xsl:if test="(./m:CNOTES)">
            <xsl:call-template name="cnotes"/>
          </xsl:if>
          <!-- **************** Start of Classical Works Table ********************* -->
          <xsl:if test="(./m:Works/m:Work)">
            <xsl:call-template name="works"/>
          </xsl:if>
          <!-- ****************** END of Classical Works Section *************** -->
        </td>
        <!-- *********** END of MuzeLeft COLUMN ********************************** -->
        <!-- *********** START of MuzeRight COLUMN ******************************* -->
        <td class="MuzeRight">
          &nbsp;
        </td>
        <!-- *********** END of MuzeRight COLUMN ************************************* -->
      </tr>
    </table>
    <!-- </xsl:if> -->
    <!-- END: ClassicalMusic Content ************************************************* -->
  </xsl:template>

  <xsl:template name="classdetail">
    <h2>Detailed Product Description: Classical Music</h2>
    <table  class="MuzeLeft">
      <xsl:call-template name="ctitle"/>
      <xsl:call-template name="crelease"/>
      <xsl:call-template name="cdistribute"/>
      <xsl:call-template name="crecording"/>
      <xsl:call-template name="cSPAR"/>
      <xsl:call-template name="cimports"/>
    </table>
  </xsl:template>

  <xsl:template name="ctitle">
    <!-- ******************** START of TITLE Section *********************-->
    <xsl:for-each select="./m:TITLE">
      <xsl:if test="(.) != ''">
        <tr>
          <td class="label">Title: </td>
          <td class="value2">
            <xsl:value-of select="../m:TITLE"/>
          </td>
        </tr>
      </xsl:if>
    </xsl:for-each>
    <!-- ******************** END of TITLE Section *********************-->
  </xsl:template>

  <xsl:template name="crelease">
    <!-- ******************** START of LABELNAME Section *********************-->
    <xsl:for-each select="./m:LABELNAME">
      <xsl:if test="(.) != ''">
        <tr>
          <td class="label">Record Label: </td>
          <td class="value2">
            <xsl:value-of select="../m:LABELNAME"/>
          </td>
        </tr>
      </xsl:if>
    </xsl:for-each>
    <!-- ******************** END of LABELNAME Section *********************-->
    <!-- ******************** START of RELEASED Section *********************-->
    <xsl:for-each select="./m:RELEASED">
      <xsl:if test="(.) != ''">
        <tr>
          <td class="label">Release Date: </td>
          <td class="value2">
            <xsl:value-of select="../m:RELEASED"/>
          </td>
        </tr>
      </xsl:if>
    </xsl:for-each>
    <!-- ******************** END of RELEASED Section *********************-->
    <!-- ******************** START of NEWREL Section *********************-->
    <xsl:for-each select="./m:NEWREL">
      <xsl:if test="(.) = 'Y'">
        <tr>
          <td class="label">&nbsp;</td>
          <td class="value2">
            <xsl:text>New Release</xsl:text>
          </td>
        </tr>
      </xsl:if>
    </xsl:for-each>
    <!-- ******************** END of NEWREL Section *********************-->
    <!-- ******************** START of REREL Section *********************-->
    <xsl:for-each select="./m:REREL">
      <xsl:if test="(.) = 'Y' or (.) = 'N'">
        <tr>
          <td class="label">&nbsp;</td>
          <td class="value2">
            <xsl:if test="(.) = 'Y'">
              <xsl:text>Previously released (as package or different packages)</xsl:text>
            </xsl:if>
            <xsl:if test="(.) = 'N'">
              <xsl:text>Not previously released</xsl:text>
            </xsl:if>
          </td>
        </tr>
      </xsl:if>
    </xsl:for-each>
    <!-- ******************** END of REREL Section *********************-->
  </xsl:template>

  <xsl:template name="cdistribute">
    <!-- ******************** START of DISTRIBUT Section *********************-->
    <xsl:for-each select="./m:DISTRIBUT">
      <xsl:if test="(.) != ''">
        <tr>
          <td class="label">US Distributor: </td>
          <td class="value2">
            <xsl:value-of select="../m:DISTRIBUT"/>
          </td>
        </tr>
      </xsl:if>
    </xsl:for-each>
    <!-- ******************** END of DISTRIBUT Section *********************-->
    <!-- ******************** START of CATALOG Section ************************** -->
    <xsl:for-each select="./m:CATALOG">
      <xsl:if test="(.) != ''">
        <tr>
          <td class="label">Catalog Number: </td>
          <td class="value2">
            <xsl:value-of select="../m:CATALOG"/>
          </td>
        </tr>
      </xsl:if>
    </xsl:for-each>
    <!-- ******************** END of CATALOG Section ************************** -->
  </xsl:template>

  <xsl:template name="crecording">
    <!-- ******************** START of NBRDISCS Section *********************-->
    <xsl:for-each select="./m:NBRDISCS">
      <xsl:if test="(.) != ''">
        <tr>
          <td class="label">Number of Discs: </td>
          <td class="value2">
            <xsl:value-of select="../m:NBRDISCS"/>
          </td>
        </tr>
      </xsl:if>
    </xsl:for-each>
    <!-- ******************** END of NBRDISCS Section *********************-->
    <!-- ******************** START of Running Time Section *********************-->
    <xsl:for-each select="./m:MINUTES">
      <xsl:if test="(../m:MINUTES) != '' or (../m:SECONDS) != '' or (../m:HOURS) != ''">
        <tr>
          <td class="label">Running Time: </td>
          <td class="value2">
            <xsl:if test="(../m:HOURS) != ''">
              <xsl:value-of select="../m:HOURS"/>
              <xsl:text> Hour</xsl:text>
              <xsl:if test="(../m:HOURS) != '1'">
                <xsl:text>s </xsl:text>
              </xsl:if>
              <xsl:if test="(../m:HOURS) = '1'">
                <xsl:text> </xsl:text>
              </xsl:if>
            </xsl:if>
            <xsl:if test="(../m:MINUTES) != ''">

              <xsl:value-of select="../m:MINUTES"/>
              <xsl:text> Minute</xsl:text>
              <xsl:if test="(../m:MINUTES) != '1'">
                <xsl:text>s </xsl:text>
              </xsl:if>
              <xsl:if test="(../m:MINUTES) = '1'">
                <xsl:text> </xsl:text>
              </xsl:if>
            </xsl:if>
            <xsl:if test="(../m:SECONDS) != ''">
              <xsl:value-of select="../m:SECONDS"/>
              <xsl:text> Seconds</xsl:text>
            </xsl:if>
          </td>
        </tr>
      </xsl:if>
    </xsl:for-each>
    <!-- ******************** END of Running Time Section *********************-->
    <!-- ******************** START of MONOSTEREO Section *********************-->
    <xsl:for-each select="./m:MONOSTEREO">
      <xsl:if test="(.) != ''">
        <tr>
          <td class="label">Stereo/Mono: </td>
          <td class="value2">
            <xsl:value-of select="../m:MONOSTEREO"/>
          </td>
        </tr>
      </xsl:if>
    </xsl:for-each>
    <!-- ******************** END of MONOSTEREO Section *********************-->
  </xsl:template>

  <xsl:template name="cSPAR">
    <!-- ******************** START of SPAR Section *********************-->
    <xsl:for-each select="./m:SPAR">
      <xsl:if test="(.) != 'n/a'">
        <tr>
          <td class="label">SPAR Recording Mix: </td>
          <td class="value2">
            <xsl:choose>
              <xsl:when test="(.) = 'AAA'">
                <xsl:value-of select="."/>
                <xsl:text>: Analog Recording; Analog Mixing; Analog Mastering</xsl:text>
              </xsl:when>
              <xsl:when test="(.) = 'AAD'">
                <xsl:value-of select="."/>
                <xsl:text>: Analog Recording; Analog Mixing; Digital Mastering</xsl:text>
              </xsl:when>
              <xsl:when test="(.) = 'ADA'">
                <xsl:value-of select="."/>
                <xsl:text>: Analog Recording; Digital Mixing; Analog Mastering</xsl:text>
              </xsl:when>
              <xsl:when test="(.) = 'ADD'">
                <xsl:value-of select="."/>
                <xsl:text>: Analog Recording; Digital Mixing; Digital Mastering</xsl:text>
              </xsl:when>
              <xsl:when test="(.) = 'DAA'">
                <xsl:value-of select="."/>
                <xsl:text>: Digital Recording; Analog Mixing; Analog Mastering</xsl:text>
              </xsl:when>
              <xsl:when test="(.) = 'DAD'">
                <xsl:value-of select="."/>
                <xsl:text>: Digital Recording; Analog Mixing; Digital Mastering</xsl:text>
              </xsl:when>
              <xsl:when test="(.) = 'DDA'">
                <xsl:value-of select="."/>
                <xsl:text>: Digital Recording; Digital Mixing; Analog Mastering</xsl:text>
              </xsl:when>
              <xsl:when test="(.) = 'DDD'">
                <xsl:value-of select="."/>
                <xsl:text>: Digital Recording; Digital Mixing; Digital Mastering</xsl:text>
              </xsl:when>
            </xsl:choose>
          </td>
        </tr>
      </xsl:if>
    </xsl:for-each>
    <!-- ******************** END of SPAR Section *********************-->
  </xsl:template>

  <xsl:template name="cimports">
    <!-- ******************** START of IMPORT Section *********************-->
    <xsl:for-each select="./m:IMPORT">
      <xsl:if test="(../m:IMPORT) != '' or (../m:AREA) != ''">
        <tr>
          <td class="label">Country of Origin: </td>
          <td class="value2">
            <xsl:if test="(../m:IMPORT) = 'Y'">
              <xsl:text>Import</xsl:text>
              <xsl:if test="(../m:AREA) != ''">
                <xsl:text> from </xsl:text>
                <xsl:value-of select="../m:AREA"/>
              </xsl:if>
            </xsl:if>

            <xsl:if test="(../m:IMPORT) = 'N'">
              <xsl:text>Domestic</xsl:text>
              <xsl:if test="(../m:AREA) != ''">
                <xsl:text> to </xsl:text>
                <xsl:value-of select="../m:AREA"/>
              </xsl:if>
            </xsl:if>
          </td>
        </tr>
      </xsl:if>
    </xsl:for-each>
    <!-- ******************** END of IMPORT Section *********************-->
  </xsl:template>

  <xsl:template name="cnotes">
    <h2>Commentary and Reviews</h2>
    <table class="MuzeLeft">
      <xsl:for-each select="./m:CNOTES">
        <xsl:if test="(.) != ''">
          <tr>
            <td class="label">Description: </td>
            <td class="value2">
              <xsl:value-of select="../m:CNOTES"/>
            </td>
          </tr>
        </xsl:if>
      </xsl:for-each>
    </table>
  </xsl:template>

  <xsl:template name="works">
    <h2>Classical Works</h2>
    <table class="MuzeLeft">
      <xsl:for-each select="./m:Works/m:Work">
        <xsl:if test="(m:TITLE) != ''">
          <xsl:call-template name="work"/>
        </xsl:if>
      </xsl:for-each>
    </table>
  </xsl:template>

  <xsl:template name="work">
    <tr>
      <td class="label" >
        Work #<xsl:value-of select="position()"/>:
      </td>
      <td class="value2" >
        <xsl:if test="(m:TITLE) != ''">
          <span class="Muzebold">
            <xsl:value-of select="m:TITLE"/>
          </span>
          <br/>
        </xsl:if>
        <xsl:if test="(m:COMPOSER) != ''">
          <span class="grayw">Composer: </span>
          <xsl:value-of select="m:COMPOSER"/>
          <br/>
        </xsl:if>
        <xsl:if test="(m:PERIOD) != ''">
          <span class="grayw">Period: </span>
          <xsl:value-of select="m:PERIOD"/>
          <br/>
        </xsl:if>
        <xsl:if test="(m:FORMGENRE) != ''">
          <span class="grayw">Musical Form Genre: </span>
          <xsl:value-of select="m:FORMGENRE"/>
          <br/>
        </xsl:if>
        <xsl:if test="(m:MINUTES) != '' or (m:SECONDS) != ''">
          <span class="grayw">Running Time: </span>
          <xsl:if test="(m:MINUTES) != ''">
            <xsl:value-of select="m:MINUTES"/>
            <xsl:text> Minutes </xsl:text>
          </xsl:if>
          <xsl:if test="(m:SECONDS) != ''">
            <xsl:value-of select="m:SECONDS"/>
            <xsl:text> Seconds</xsl:text>
          </xsl:if>
          <br/>
        </xsl:if>
        <xsl:if test="(m:WRITTEN) != ''">
          <span class="grayw">Date Written/Composed: </span>
          <xsl:value-of select="m:WRITTEN"/>
          <br/>
        </xsl:if>
        <xsl:if test="(m:COUNTRY) != ''">
          <span class="grayw">Location where Written/Composed: </span>
          <xsl:value-of select="m:COUNTRY"/>
          <br/>
        </xsl:if>
        <xsl:if test="(m:VENUE) != ''">
          <span class="grayw">Recording Venue: </span>
          <xsl:value-of select="VENUE"/>
          <br/>
        </xsl:if>
        <xsl:if test="(m:LANGUAGE) != ''">
          <span class="grayw">Language performed in: </span>
          <xsl:value-of select="m:LANGUAGE"/>
          <br/>
        </xsl:if>
        <xsl:if test="(m:STUDIOLIVE) != ''">
          <span class="grayw">Recording Environment: </span>
          <xsl:if test="(m:STUDIOLIVE) = 'Live'">
            <xsl:text>Live Performance</xsl:text>
            <br/>
          </xsl:if>
          <xsl:if test="(m:STUDIOLIVE) = 'Studio'">
            <xsl:text>Studio Recording</xsl:text>
            <br/>
          </xsl:if>
          <xsl:if test="(m:STUDIOLIVE) = 'Mixed'">
            <xsl:text>Mix of Live Performance and Studio Recordings</xsl:text>
            <br/>
          </xsl:if>
        </xsl:if>
        <xsl:if test="(m:RECORDTE) != ''">
          <span class="grayw">Date of Performance: </span>
          <xsl:value-of select="m:RECORDTE"/>
          <br/>
        </xsl:if>
        <xsl:if test="(m:COMPLETE) = 'Y'">
          <span class="grayw">Completeness: </span>Complete Classical Work<br/>
        </xsl:if>
        <xsl:if test="(m:COMPLETE) = 'N'">
          <span class="grayw">Completeness: </span>Excerpt, not complete Classical Work<xsl:value-of select="m:COMPLETE"/><br/>
        </xsl:if>
        <xsl:if test="(m:WNOTES) != ''">
          <span class="grayw">Related Notes: </span>
          <xsl:value-of select="m:WNOTES"/>
          <br/>
        </xsl:if>
      </td>
    </tr>
  </xsl:template>

  <!-- *************** SECTION FOUR is GameRelease ************************************* -->
  <xsl:template name="Games" match="//m:GameRelease">
    <table class="Muze">
      <tr>
        <!-- *********** START of MuzeLeft COLUMN **************************************** -->
        <td class="MuzeLeft">
          <!-- *********************** START of Games Table ******************************** -->
          <!--            <xsl:if test="$DoGames = '1'">
-->
          <h2>Detailed Product Description: Games</h2>
          <table  class="MuzeLeft">
            <tr>
              <td colspan="2">
                Baker &amp; Taylor is currently unable to provide Muze enhanced-data
                for Games.<br />We apologize for any inconvenience.
              </td>
            </tr>
          </table>
          <!--            </xsl:if>
-->
          <!-- *********************** END of Games Table ********************************** -->
        </td>
        <!-- *********** END of MuzeLeft COLUMN ****************************************** -->
        <!-- *********** START of MuzeRight COLUMN *************************************** -->
        <td class="MuzeRight">
          &nbsp;
        </td>
        <!-- *********** END of MuzeRight COLUMN **************************************** -->
      </tr>
    </table>
    <!-- </xsl:if> -->
    <!-- END: GameRelease Content **************************************************** -->
    <!-- ***************************************************************************** -->
    <!-- ***************************************************************************** -->
  </xsl:template>

  <!-- template that actually does the conversion -->
  <xsl:template name="lf2br">
    <!-- import $StringToTransform -->
    <xsl:param name="StringToTransform"/>
    <xsl:choose>
      <!-- string contains linefeed -->
      <xsl:when test="contains($StringToTransform,'&#xA;')">
        <!-- output substring that comes before the first linefeed -->
        <!-- note: use of substring-before() function means        -->
        <!-- $StringToTransform will be treated as a string,       -->
        <!-- even if it is a node-set or result tree fragment.     -->
        <!-- So hopefully $StringToTransform is really a string!   -->
        <xsl:value-of select="substring-before($StringToTransform,'&#xA;')"/>
        <!-- by putting a 'br' element in the result tree instead  -->
        <!-- of the linefeed character, a <br> will be output at   -->
        <!-- that point in the HTML                                -->
        <br/>
        <!-- repeat for the remainder of the original string -->
        <xsl:call-template name="lf2br">
          <xsl:with-param name="StringToTransform">
            <xsl:value-of select="substring-after($StringToTransform,'&#xA;')"/>
          </xsl:with-param>
        </xsl:call-template>
      </xsl:when>
      <!-- string does not contain newline, so just output it -->
      <xsl:otherwise>
        <xsl:value-of select="$StringToTransform"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>



</xsl:stylesheet>