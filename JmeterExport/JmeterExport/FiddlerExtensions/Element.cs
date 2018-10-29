using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JmeterExport.FiddlerExtensions
{
    public class Element
    {
        private StringBuilder stringBuilder;

        public Element()
        {
        }

        public string addXmlHead(string xmlBody)
        {
            this.stringBuilder = new StringBuilder();
            stringBuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            stringBuilder.Append(xmlBody);

            return stringBuilder.ToString();
        }

        public string surroundByTestPlan(string content, string testPlanName="测试计划")
        {
            this.stringBuilder = new StringBuilder();
            stringBuilder.Append("<jmeterTestPlan version=\"1.2\" properties=\"3.2\">");
            stringBuilder.Append("<hashTree>");
            stringBuilder.Append(string.Format("<TestPlan guiclass=\"TestPlanGui\" testclass=\"TestPlan\" testname=\"{0}\" enabled=\"true\">", testPlanName));
            stringBuilder.Append("<stringProp name=\"TestPlan.comments\"></stringProp>");
            stringBuilder.Append("<boolProp name=\"TestPlan.functional_mode\">false</boolProp>");
            stringBuilder.Append("<boolProp name=\"TestPlan.serialize_threadgroups\">false</boolProp>");
            stringBuilder.Append("<elementProp name=\"TestPlan.user_defined_variables\" elementType=\"Arguments\" guiclass=\"ArgumentsPanel\" testclass=\"Arguments\" testname=\"用户定义的变量\" enabled=\"true\">");
            stringBuilder.Append("<collectionProp name=\"Arguments.arguments\"/>");
            stringBuilder.Append("</elementProp>");
            stringBuilder.Append("<stringProp name=\"TestPlan.user_define_classpath\"></stringProp>");
            stringBuilder.Append("</TestPlan>");
            stringBuilder.Append("<hashTree>");
            if (content != null)
            {
                stringBuilder.Append(content);
            }
            stringBuilder.Append("</hashTree>");
            stringBuilder.Append("<WorkBench guiclass=\"WorkBenchGui\" testclass=\"WorkBench\" testname=\"工作台\" enabled=\"true\">");
            stringBuilder.Append("<boolProp name=\"WorkBench.save\">true</boolProp>");
            stringBuilder.Append("</WorkBench>");
            stringBuilder.Append("<hashTree/>");
            stringBuilder.Append("</hashTree>");
            stringBuilder.Append("</jmeterTestPlan>");
            return stringBuilder.ToString();
        }

        public string addConfigTestElement(string content, string ip, string port, string encoding = "utf-8", string protocol = "http", string elementName = "HTTP请求默认值")
        {
            this.stringBuilder = new StringBuilder();
            if (content != null)
            {
                stringBuilder.Append(content);
            }
            stringBuilder.Append(string.Format("<ConfigTestElement guiclass=\"HttpDefaultsGui\" testclass=\"ConfigTestElement\" testname = \"{0}\" enabled = \"true\">", elementName));
            stringBuilder.Append("<elementProp name=\"HTTPsampler.Arguments\" elementType=\"Arguments\" guiclass=\"HTTPArgumentsPanel\" testclass=\"Arguments\" testname=\"用户定义的变量\" enabled=\"true\">");
            stringBuilder.Append("<collectionProp name=\"Arguments.arguments\"/>");
            stringBuilder.Append("</elementProp>");
            stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.domain\">{0}</stringProp>", ip));
            stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.port\">{0}</stringProp>", port));
            stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.protocol\">{0}</stringProp>", protocol));
            stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.contentEncoding\">{0}</stringProp>", encoding));
            stringBuilder.Append("<stringProp name=\"HTTPSampler.path\"></stringProp>");
            stringBuilder.Append("<stringProp name=\"HTTPSampler.concurrentPool\">6</stringProp>");
            stringBuilder.Append("<stringProp name=\"HTTPSampler.connect_timeout\"></stringProp>");
            stringBuilder.Append("<stringProp name=\"HTTPSampler.response_timeout\"></stringProp>");
            stringBuilder.Append("</ConfigTestElement>");
            stringBuilder.Append("<hashTree/>");

            return stringBuilder.ToString();
        }

        public string addCookieManager(string content, string elementName="HTTP Cookie 管理器")
        {
            this.stringBuilder = new StringBuilder();
            if (content != null)
            {
                stringBuilder.Append(content);
            }
            stringBuilder.Append(string.Format("<CookieManager guiclass=\"CookiePanel\" testclass=\"CookieManager\" testname=\"{0}\" enabled=\"true\">", elementName));
            stringBuilder.Append("<collectionProp name=\"CookieManager.cookies\"/>");
            stringBuilder.Append("<boolProp name=\"CookieManager.clearEachIteration\">false</boolProp>");
            stringBuilder.Append("</CookieManager>");
            stringBuilder.Append("<hashTree/>");

            return stringBuilder.ToString();
        }

        public string addArguments(string content, string elementName= "用户定义的变量")
        {
            this.stringBuilder = new StringBuilder();
            if (content != null)
            {
                stringBuilder.Append(content);
            }
            stringBuilder.Append(string.Format("<Arguments guiclass=\"ArgumentsPanel\" testclass=\"Arguments\" testname=\"{0}\" enabled=\"true\">", elementName));
            stringBuilder.Append("<collectionProp name=\"Arguments.arguments\"/>");
            stringBuilder.Append("</Arguments>");
            stringBuilder.Append("<hashTree/>");

            return stringBuilder.ToString();
        }

        public string surroundByThreadGroup(string content, string elementName="线程组")
        {
            this.stringBuilder = new StringBuilder();
            stringBuilder.Append(string.Format("<ThreadGroup guiclass=\"ThreadGroupGui\" testclass=\"ThreadGroup\" testname=\"{0}\" enabled=\"true\">", elementName));
            stringBuilder.Append("<stringProp name=\"ThreadGroup.on_sample_error\">continue</stringProp>");
            stringBuilder.Append("<elementProp name=\"ThreadGroup.main_controller\" elementType=\"LoopController\" guiclass=\"LoopControlPanel\" testclass=\"LoopController\" testname=\"循环控制器\" enabled=\"true\">");
            stringBuilder.Append("<boolProp name=\"LoopController.continue_forever\">false</boolProp>");
            stringBuilder.Append("<stringProp name=\"LoopController.loops\">1</stringProp>");
            stringBuilder.Append("</elementProp>");
            stringBuilder.Append("<stringProp name=\"ThreadGroup.num_threads\">1</stringProp>");
            stringBuilder.Append("<stringProp name=\"ThreadGroup.ramp_time\">1</stringProp>");
            Int64 timeStamp = Convert.ToInt64(DateTime.Now.Subtract(DateTime.Parse("1970-1-1")).TotalMilliseconds);
            stringBuilder.Append(string.Format("<longProp name=\"ThreadGroup.start_time\">{0}</longProp>", timeStamp));
            stringBuilder.Append(string.Format("<longProp name=\"ThreadGroup.end_time\">{0}</longProp>", timeStamp));
            stringBuilder.Append("<boolProp name=\"ThreadGroup.scheduler\">false</boolProp>");
            stringBuilder.Append("<stringProp name=\"ThreadGroup.duration\"></stringProp>");
            stringBuilder.Append("<stringProp name=\"ThreadGroup.delay\"></stringProp>");
            stringBuilder.Append("</ThreadGroup>");
            stringBuilder.Append("<hashTree>");
            stringBuilder.Append(content);
            stringBuilder.Append("</hashTree>");

            return stringBuilder.ToString();
        }

        
        public string addViewResultTree(string content, string elementName="察看结果树")
        {
            this.stringBuilder = new StringBuilder();
            if (content != null)
            {
                stringBuilder.Append(content);
            }
            stringBuilder.Append(string.Format("<ResultCollector guiclass = \"ViewResultsFullVisualizer\" testclass=\"ResultCollector\" testname=\"{0}\" enabled=\"true\">", elementName));
            stringBuilder.Append("<boolProp name=\"ResultCollector.error_logging\">false</boolProp>");
            stringBuilder.Append("<objProp>");
            stringBuilder.Append("<name>saveConfig</name>");
            stringBuilder.Append("<value class=\"SampleSaveConfiguration\">");
            stringBuilder.Append("<time>true</time>");
            stringBuilder.Append("<latency>true</latency>");
            stringBuilder.Append("<timestamp>true</timestamp>");
            stringBuilder.Append("<success>true</success>");
            stringBuilder.Append("<success>true</success>");
            stringBuilder.Append("<label>true</label>");
            stringBuilder.Append("<code>true</code>");
            stringBuilder.Append("<message>true</message>");
            stringBuilder.Append("<threadName>true</threadName>");
            stringBuilder.Append("<dataType>true</dataType>");
            stringBuilder.Append("<encoding>false</encoding>");
            stringBuilder.Append("<assertions>true</assertions>");
            stringBuilder.Append("<subresults>true</subresults>");
            stringBuilder.Append("<responseData>false</responseData>");
            stringBuilder.Append("<samplerData>false</samplerData>");
            stringBuilder.Append("<xml>false</xml>");
            stringBuilder.Append("<fieldNames>true</fieldNames>");
            stringBuilder.Append("<responseHeaders>false</responseHeaders>");
            stringBuilder.Append("<requestHeaders>false</requestHeaders>");
            stringBuilder.Append("<responseDataOnError>false</responseDataOnError>");
            stringBuilder.Append("<saveAssertionResultsFailureMessage>true</saveAssertionResultsFailureMessage>");
            stringBuilder.Append("<assertionsResultsToSave>0</assertionsResultsToSave>");
            stringBuilder.Append("<bytes>true</bytes>");
            stringBuilder.Append("<sentBytes>true</sentBytes>");
            stringBuilder.Append("<threadCounts>true</threadCounts>");
            stringBuilder.Append("<idleTime>true</idleTime>");
            stringBuilder.Append("<connectTime>true</connectTime>");
            stringBuilder.Append("</value>");
            stringBuilder.Append("</objProp>");
            stringBuilder.Append("<stringProp name=\"filename\"></stringProp>");
            stringBuilder.Append("</ResultCollector>");
            stringBuilder.Append("<hashTree/>");

            return stringBuilder.ToString();
        }

        public string addAssertionResult(string content, string elementName="断言结果")
        {
            this.stringBuilder = new StringBuilder();
            if (content != null)
            {
                stringBuilder.Append(content);
            }
            stringBuilder.Append(string.Format("<ResultCollector guiclass=\"AssertionVisualizer\" testclass=\"ResultCollector\" testname=\"{0}\" enabled=\"true\">", elementName));
            stringBuilder.Append("<boolProp name=\"ResultCollector.error_loggin\">false</boolProp>");
            stringBuilder.Append("<objProp>");
            stringBuilder.Append("<name>saveConfig</name>");
            stringBuilder.Append("<value class=\"SampleSaveConfiguration\">");
            stringBuilder.Append("<time>true</time>");
            stringBuilder.Append("<latency>true</latency>");
            stringBuilder.Append("<timestamp>true</timestamp>");
            stringBuilder.Append("<success>true</success>");
            stringBuilder.Append("<label>true</label>");
            stringBuilder.Append("<code>true</code>");
            stringBuilder.Append("<message>true</message>");
            stringBuilder.Append("<threadName>true</threadName>");
            stringBuilder.Append("<dataType>true</dataType>");
            stringBuilder.Append("<encoding>false</encoding>");
            stringBuilder.Append("<assertions>true</assertions>");
            stringBuilder.Append("<subresults>true</subresults>");
            stringBuilder.Append("<responseData>false</responseData>");
            stringBuilder.Append("<samplerData>false</samplerData>");
            stringBuilder.Append("<xml>false</xml>");
            stringBuilder.Append("<fieldNames>true</fieldNames>");
            stringBuilder.Append("<responseHeaders>false</responseHeaders>");
            stringBuilder.Append("<requestHeaders>false</requestHeaders>");
            stringBuilder.Append("<responseDataOnError>false</responseDataOnError>");
            stringBuilder.Append("<saveAssertionResultsFailureMessage>true</saveAssertionResultsFailureMessage>");
            stringBuilder.Append("<assertionsResultsToSave>0</assertionsResultsToSave>");
            stringBuilder.Append("<bytes>true</bytes>");
            stringBuilder.Append("<sentBytes>true</sentBytes>");
            stringBuilder.Append("<threadCounts>true</threadCounts>");
            stringBuilder.Append("<idleTime>true</idleTime>");
            stringBuilder.Append("<connectTime>true</connectTime>");
            stringBuilder.Append("</value>");
            stringBuilder.Append("</objProp>");
            stringBuilder.Append("<stringProp name=\"filename\"></stringProp>");
            stringBuilder.Append("</ResultCollector>");
            stringBuilder.Append("<hashTree/>");

            return stringBuilder.ToString();
        }   

        public string surroundByHTTPSamplerProxy(string content, string ip, string port, string protocol, string method, string path, string value, string elementName)
        {
            this.stringBuilder = new StringBuilder();
            stringBuilder.Append(string.Format("<HTTPSamplerProxy guiclass=\"HttpTestSampleGui\" testclass=\"HTTPSamplerProxy\" testname=\"{0}\" enabled=\"true\">", elementName));
            stringBuilder.Append("<boolProp name=\"HTTPSampler.postBodyRaw\">true</boolProp>");
            stringBuilder.Append("<elementProp name=\"HTTPsampler.Arguments\" elementType=\"Arguments\">");
            stringBuilder.Append("<collectionProp name=\"Arguments.arguments\">");
            stringBuilder.Append("<elementProp name=\"\" elementType=\"HTTPArgument\">");
            stringBuilder.Append("<boolProp name=\"HTTPArgument.always_encode\">false</boolProp>");
            stringBuilder.Append(string.Format("<stringProp name=\"Argument.value\">{0}</stringProp>", value));
            stringBuilder.Append("<stringProp name=\"Argument.metadata\">=</stringProp>");
            stringBuilder.Append("</elementProp>");
            stringBuilder.Append("</collectionProp>");
            stringBuilder.Append("</elementProp>");
            stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.domain\">{0}</stringProp>", ip));
            stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.port\">{0}</stringProp>", port));
            stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.protocol\">{0}</stringProp>", protocol));
            stringBuilder.Append("<stringProp name=\"HTTPSampler.contentEncoding\"></stringProp>");
            stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.path\">{0}</stringProp>", path));
            stringBuilder.Append(string.Format("<stringProp name=\"HTTPSampler.method\">{0}</stringProp>", method));
            stringBuilder.Append("<boolProp name=\"HTTPSampler.follow_redirects\">true</boolProp>");
            stringBuilder.Append("<boolProp name=\"HTTPSampler.auto_redirects\">false</boolProp>");
            stringBuilder.Append("<boolProp name=\"HTTPSampler.use_keepalive\">true</boolProp>");
            stringBuilder.Append("<boolProp name=\"HTTPSampler.DO_MULTIPART_POST\">false</boolProp>");
            stringBuilder.Append("<stringProp name=\"HTTPSampler.embedded_url_re\"></stringProp>");
            stringBuilder.Append("<stringProp name=\"HTTPSampler.connect_timeout\"></stringProp>");
            stringBuilder.Append("<stringProp name=\"HTTPSampler.response_timeout\"></stringProp>");
            stringBuilder.Append("</HTTPSamplerProxy>");
            stringBuilder.Append("<hashTree>");
            stringBuilder.Append(content);
            stringBuilder.Append("</hashTree>");


            return stringBuilder.ToString();
        }
     
        public string addJSONPathAssertion(string content, string jsonPath= "$.stat", string expectedValue="OK", string elementName="验证响应结果")
        {
            this.stringBuilder = new StringBuilder();
            if (content != null)
            {
                stringBuilder.Append(content);
            }
            stringBuilder.Append(string.Format("<com.atlantbh.jmeter.plugins.jsonutils.jsonpathassertion.JSONPathAssertion guiclass = \"com.atlantbh.jmeter.plugins.jsonutils.jsonpathassertion.gui.JSONPathAssertionGui\" testclass=\"com.atlantbh.jmeter.plugins.jsonutils.jsonpathassertion.JSONPathAssertion\" testname=\"{0}\" enabled=\"true\">", elementName));
            stringBuilder.Append(string.Format("<stringProp name=\"JSON_PATH\">{0}</stringProp>", jsonPath));
            stringBuilder.Append(string.Format("<stringProp name=\"EXPECTED_VALUE\">{0}</stringProp>", expectedValue));
            stringBuilder.Append("<boolProp name=\"JSONVALIDATION\">true</boolProp>");
            stringBuilder.Append("<boolProp name=\"EXPECT_NULL\">false</boolProp>");
            stringBuilder.Append("<boolProp name=\"INVERT\">false</boolProp>");
            stringBuilder.Append("<boolProp name=\"ISREGEX\">true</boolProp>");
            stringBuilder.Append("</com.atlantbh.jmeter.plugins.jsonutils.jsonpathassertion.JSONPathAssertion>");
            stringBuilder.Append("<hashTree/>");

            return stringBuilder.ToString();
        }

    }
}
