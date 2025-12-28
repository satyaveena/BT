namespace RetrieveReconciliationFirstData
{
    using System;
    using System.Collections.Generic;
    using Microsoft.BizTalk.PipelineOM;
    using Microsoft.BizTalk.Component;
    using Microsoft.BizTalk.Component.Interop;
    
    
    public sealed class XMLReceivePipeline : Microsoft.BizTalk.PipelineOM.ReceivePipeline
    {
        
        private const string _strPipeline = "<?xml version=\"1.0\" encoding=\"utf-16\"?><Document xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instanc"+
"e\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" MajorVersion=\"1\" MinorVersion=\"0\">  <Description /> "+
" <CategoryId>f66b9f5e-43ff-4f5f-ba46-885348ae1b4e</CategoryId>  <FriendlyName>Receive</FriendlyName>"+
"  <Stages>    <Stage>      <PolicyFileStage _locAttrData=\"Name\" _locID=\"1\" Name=\"Decode\" minOccurs=\""+
"0\" maxOccurs=\"-1\" execMethod=\"All\" stageId=\"9d0e4103-4cce-4536-83fa-4a5040674ad6\" />      <Component"+
"s>        <Component>          <Name>BTNextGen.BizTalk.PipelineComponents.Common.ExcelDecoder,BTNext"+
"Gen.BizTalk.PipelineComponents.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=be3013815dea"+
"efc7</Name>          <ComponentName>Excel Decoder</ComponentName>          <Description>Convert Exce"+
"l to XML</Description>          <Version>1.0.0.0</Version>          <Properties>            <Propert"+
"y Name=\"ConnectionString\">              <Value xsi:type=\"xsd:string\" />            </Property>      "+
"      <Property Name=\"TempDropFolderLocation\">              <Value xsi:type=\"xsd:string\" />         "+
"   </Property>            <Property Name=\"SqlStatement\">              <Value xsi:type=\"xsd:string\" /"+
">            </Property>            <Property Name=\"DeleteTempMessages\">              <Value xsi:typ"+
"e=\"xsd:boolean\">true</Value>            </Property>            <Property Name=\"RootNodeName\">       "+
"       <Value xsi:type=\"xsd:string\" />            </Property>            <Property Name=\"NameSpace\">"+
"              <Value xsi:type=\"xsd:string\" />            </Property>            <Property Name=\"Data"+
"NodeName\">              <Value xsi:type=\"xsd:string\" />            </Property>            <Property "+
"Name=\"Filter\">              <Value xsi:type=\"xsd:string\" />            </Property>          </Proper"+
"ties>          <CachedDisplayName>Excel Decoder</CachedDisplayName>          <CachedIsManaged>true</"+
"CachedIsManaged>        </Component>      </Components>    </Stage>    <Stage>      <PolicyFileStage"+
" _locAttrData=\"Name\" _locID=\"2\" Name=\"Disassemble\" minOccurs=\"0\" maxOccurs=\"-1\" execMethod=\"FirstMat"+
"ch\" stageId=\"9d0e4105-4cce-4536-83fa-4a5040674ad6\" />      <Components />    </Stage>    <Stage>    "+
"  <PolicyFileStage _locAttrData=\"Name\" _locID=\"3\" Name=\"Validate\" minOccurs=\"0\" maxOccurs=\"-1\" execM"+
"ethod=\"All\" stageId=\"9d0e410d-4cce-4536-83fa-4a5040674ad6\" />      <Components />    </Stage>    <St"+
"age>      <PolicyFileStage _locAttrData=\"Name\" _locID=\"4\" Name=\"ResolveParty\" minOccurs=\"0\" maxOccur"+
"s=\"-1\" execMethod=\"All\" stageId=\"9d0e410e-4cce-4536-83fa-4a5040674ad6\" />      <Components />    </S"+
"tage>  </Stages></Document>";
        
        private const string _versionDependentGuid = "f0305264-55e0-44ba-a188-59544031b771";
        
        public XMLReceivePipeline()
        {
            Microsoft.BizTalk.PipelineOM.Stage stage = this.AddStage(new System.Guid("9d0e4103-4cce-4536-83fa-4a5040674ad6"), Microsoft.BizTalk.PipelineOM.ExecutionMode.all);
            IBaseComponent comp0 = Microsoft.BizTalk.PipelineOM.PipelineManager.CreateComponent("BTNextGen.BizTalk.PipelineComponents.Common.ExcelDecoder,BTNextGen.BizTalk.PipelineComponents.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=be3013815deaefc7");;
            if (comp0 is IPersistPropertyBag)
            {
                string comp0XmlProperties = "<?xml version=\"1.0\" encoding=\"utf-16\"?><PropertyBag xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-inst"+
"ance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">  <Properties>    <Property Name=\"ConnectionStrin"+
"g\">      <Value xsi:type=\"xsd:string\" />    </Property>    <Property Name=\"TempDropFolderLocation\"> "+
"     <Value xsi:type=\"xsd:string\" />    </Property>    <Property Name=\"SqlStatement\">      <Value xs"+
"i:type=\"xsd:string\" />    </Property>    <Property Name=\"DeleteTempMessages\">      <Value xsi:type=\""+
"xsd:boolean\">true</Value>    </Property>    <Property Name=\"RootNodeName\">      <Value xsi:type=\"xsd"+
":string\" />    </Property>    <Property Name=\"NameSpace\">      <Value xsi:type=\"xsd:string\" />    </"+
"Property>    <Property Name=\"DataNodeName\">      <Value xsi:type=\"xsd:string\" />    </Property>    <"+
"Property Name=\"Filter\">      <Value xsi:type=\"xsd:string\" />    </Property>  </Properties></Property"+
"Bag>";
                PropertyBag pb = PropertyBag.DeserializeFromXml(comp0XmlProperties);;
                ((IPersistPropertyBag)(comp0)).Load(pb, 0);
            }
            this.AddComponent(stage, comp0);
        }
        
        public override string XmlContent
        {
            get
            {
                return _strPipeline;
            }
        }
        
        public override System.Guid VersionDependentGuid
        {
            get
            {
                return new System.Guid(_versionDependentGuid);
            }
        }
    }
}
