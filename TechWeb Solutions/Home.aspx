<%@ Page Title="Home - TechWeb Solutions" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="TechWeb_Solutions.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .home-container {
            max-width: 900px;
            margin: 0 auto;
            padding: 40px 20px;
            background-color: #f9f9f9;
            border-radius: 12px;
            box-shadow: 0 0 12px rgba(0, 0, 0, 0.1);
        }

        .home-container h2 {
            color: #003366;
            font-size: 32px;
            margin-bottom: 20px;
            text-align: center;
        }

        .home-container p {
            font-size: 18px;
            line-height: 1.6;
            color: #333;
            text-align: justify;
        }

        .highlight {
            color: #005599;
            font-weight: bold;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="home-container">
        <h2>Welcome to <span class="highlight">TechWeb Solutions</span></h2>
        <p>
            At <span class="highlight">TechWeb Solutions</span>, we specialize in delivering high-quality web and software development services tailored to meet the unique needs of modern businesses. 
            With a passionate team of experienced developers, designers, and IT professionals, we are committed to driving digital transformation and helping our clients thrive in the digital world.
        </p>
        <p>
            Our mission is to provide scalable, secure, and user-friendly solutions that empower businesses to achieve their goals. From responsive websites to enterprise-grade applications, 
            we blend creativity with technology to turn your vision into reality.
        </p>
    </div>
</asp:Content>
