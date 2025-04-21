<%@ Page Title="Services - TechWeb Solutions" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Services.aspx.cs" Inherits="TechWeb_Solutions.Services" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .services-container {
            max-width: 900px;
            margin: 0 auto;
            padding: 40px 20px;
            background-color: #f9f9f9;
            border-radius: 12px;
            box-shadow: 0 0 12px rgba(0, 0, 0, 0.1);
        }

        .services-container h2 {
            color: #003366;
            font-size: 32px;
            margin-bottom: 25px;
            text-align: center;
        }

        .service-list {
            list-style: none;
            padding: 0;
        }

        .service-list li {
            background-color: #ffffff;
            padding: 20px;
            margin-bottom: 15px;
            border-left: 6px solid #005599;
            border-radius: 8px;
            box-shadow: 0 1px 5px rgba(0,0,0,0.1);
        }

        .service-list li strong {
            color: #005599;
            display: block;
            font-size: 20px;
            margin-bottom: 5px;
        }

        .service-list li span {
            font-size: 16px;
            color: #333;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="services-container">
        <h2>Our Services</h2>
        <ul class="service-list">
            <li>
                <strong>Web Development</strong>
                <span>We build custom websites and web applications that are secure, scalable, and tailored to your business needs.</span>
            </li>
            <li>
                <strong>Mobile Apps</strong>
                <span>Our team creates user-friendly Android and iOS mobile applications to enhance your digital reach.</span>
            </li>
            <li>
                <strong>Cloud Solutions</strong>
                <span>Deploy your services in the cloud with confidence using our scalable, secure, and reliable cloud infrastructure solutions.</span>
            </li>
            <li>
                <strong>UI/UX Design</strong>
                <span>We craft modern, responsive, and accessible interfaces focused on providing the best user experience.</span>
            </li>
        </ul>
    </div>
</asp:Content>
