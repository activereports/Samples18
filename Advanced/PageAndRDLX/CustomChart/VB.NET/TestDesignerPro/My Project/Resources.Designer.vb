'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("ActiveReports.Samples.Radar.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property cmdnewreport() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("cmdnewreport", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property cmdopen() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("cmdopen", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property cmdsave() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("cmdsave", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property cmdsaveas() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("cmdsaveas", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Untitled.rdlx.
        '''</summary>
        Friend ReadOnly Property DefaultReportNameRdlx() As String
            Get
                Return ResourceManager.GetString("DefaultReportNameRdlx", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Untitled.rpx.
        '''</summary>
        Friend ReadOnly Property DefaultReportNameRpx() As String
            Get
                Return ResourceManager.GetString("DefaultReportNameRpx", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to *.
        '''</summary>
        Friend ReadOnly Property DirtySign() As String
            Get
                Return ResourceManager.GetString("DirtySign", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property GroupEditorHide() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("GroupEditorHide", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property GroupEditorShow() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("GroupEditorShow", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Hide Group Editor.
        '''</summary>
        Friend ReadOnly Property HideGroupPanelToolTip() As String
            Get
                Return ResourceManager.GetString("HideGroupPanelToolTip", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Exit.
        '''</summary>
        Friend ReadOnly Property MenuExit() As String
            Get
                Return ResourceManager.GetString("MenuExit", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to New.
        '''</summary>
        Friend ReadOnly Property MenuNew() As String
            Get
                Return ResourceManager.GetString("MenuNew", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Open.
        '''</summary>
        Friend ReadOnly Property MenuOpen() As String
            Get
                Return ResourceManager.GetString("MenuOpen", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Save.
        '''</summary>
        Friend ReadOnly Property MenuSave() As String
            Get
                Return ResourceManager.GetString("MenuSave", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Save As.
        '''</summary>
        Friend ReadOnly Property MenuSaveAs() As String
            Get
                Return ResourceManager.GetString("MenuSaveAs", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to rdlx files|*.rdlx.
        '''</summary>
        Friend ReadOnly Property RdlxFilter() As String
            Get
                Return ResourceManager.GetString("RdlxFilter", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to rpx files|*.rpx.
        '''</summary>
        Friend ReadOnly Property RpxFilter() As String
            Get
                Return ResourceManager.GetString("RpxFilter", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to RadarChart - .
        '''</summary>
        Friend ReadOnly Property SampleNameTitle() As String
            Get
                Return ResourceManager.GetString("SampleNameTitle", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Report has been changed. Do you wish to save it?.
        '''</summary>
        Friend ReadOnly Property SaveConformation() As String
            Get
                Return ResourceManager.GetString("SaveConformation", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Show Group Editor.
        '''</summary>
        Friend ReadOnly Property ShowGroupPanelToolTip() As String
            Get
                Return ResourceManager.GetString("ShowGroupPanelToolTip", resourceCulture)
            End Get
        End Property
    End Module
End Namespace
