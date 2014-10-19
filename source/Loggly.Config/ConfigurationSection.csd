<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="d0ed9acb-0435-4532-afdd-b5115bc4d562" namespace="Loggly.Config" xmlSchemaNamespace="Loggly" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="LogglyAppConfig" namespace="Loggly.Config" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="loggly">
      <attributeProperties>
        <attributeProperty name="CustomerToken" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="customerToken" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="ThrowExceptions" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="throwExceptions" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/Boolean" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="Tags" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="tags" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/Tags" />
          </type>
        </elementProperty>
        <elementProperty name="Transport" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="transport" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/Transport" />
          </type>
        </elementProperty>
        <elementProperty name="Search" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="search" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/SearchAppConfig" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElementCollection name="ComplexTagCollection" namespace="Loggly.Config" xmlItemName="tag" codeGenOptions="AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/ComplexTagAppConfig" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="ComplexTagAppConfig" namespace="Loggly.Config">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Assembly" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="assembly" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Type" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="type" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Formatter" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="formatter" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="SimpleTagCollection" xmlItemName="tag" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/SimpleTagAppConfig" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="SimpleTagAppConfig" namespace="Loggly.Config">
      <attributeProperties>
        <attributeProperty name="Value" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="value" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElement name="Tags">
      <elementProperties>
        <elementProperty name="Simple" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="simple" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/SimpleTagCollection" />
          </type>
        </elementProperty>
        <elementProperty name="Complex" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="complex" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/ComplexTagCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElement name="Transport">
      <elementProperties>
        <elementProperty name="Http" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="http" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/HttpTransport" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElement name="HttpTransport" />
    <configurationElement name="SearchAppConfig">
      <attributeProperties>
        <attributeProperty name="Account" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="account" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Username" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="username" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Password" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="password" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/d0ed9acb-0435-4532-afdd-b5115bc4d562/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>