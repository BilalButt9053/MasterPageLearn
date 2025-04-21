<%@ Page Title="Contact - TechWeb Solutions" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="TechWeb_Solutions.Contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .contact-container {
            max-width: 700px;
            margin: 0 auto;
            padding: 20px 20px;
            background-color: #f9f9f9;
            border-radius: 12px;
            box-shadow: 0 0 12px rgba(0, 0, 0, 0.1);
        }

        .contact-container h2 {
            color: #003366;
            font-size: 32px;
            margin-bottom: 30px;
            text-align: center;
        }

        .contact-container label {
            font-size: 18px;
            color: #333;
            margin-bottom: 10px;
            display: inline-block;
        }

        .contact-container input,
        .contact-container textarea {
            width: 100%;
            padding: 12px;
            margin-bottom: 20px;
            border: 2px solid #ccc;
            border-radius: 8px;
            font-size: 16px;
        }

        .contact-container textarea {
            resize: vertical;
        }

        .contact-container input[type="submit"] {
            background-color: #005599;
            color: white;
            border: none;
            cursor: pointer;
            font-size: 18px;
            padding: 15px;
            border-radius: 8px;
            width: auto;
            margin: 0 auto;
            display: block;
        }

        .contact-container input[type="submit"]:hover {
            background-color: #003366;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="contact-container">
        <h2>Contact Us</h2>
        <div>
            <label for="name">Name:</label>
            <input type="text" id="name" name="name"  /><br />

            <label for="email">Email:</label>
            <input type="email" id="email" name="email"  /><br />

            <label for="subject">Subject:</label>
            <input type="text" id="subject" name="subject"  /><br />

            <label for="message">Message:</label>
            <textarea id="message" name="message" rows="5" ></textarea><br />

            <input type="submit" value="Send" />
        </div>
    </div>
</asp:Content>
