
## Note

- I built the project from scratch, opting for a straightforward approach without relying on a layered architecture. Everything has been implemented in a basic manner due to the project's small size, eliminating the need for unnecessary complexity.

## Overview

The Extendable Customers API is a web application built with ASP.NET Core and MongoDB. 
It allows users to manage customer data with a focus on extensibility. 
The data model includes entities for Companies and Contacts, with support for adding custom fields of various types.

## Features

- **Companies:**
  - Add, read, update, and delete companies.
  - Define custom fields for companies (Text, Number, Date).
  - Filter companies based on existing and custom fields.

- **Contacts:**
  - Add, read, update, and delete contacts.
  - Associate contacts with multiple companies.
  - Define custom fields for contacts (Text, Number, Date).
  - Filter contacts based on existing and custom fields.

- **Scalability:**
  - Designed to handle millions of records without performance issues.

## Technical Details

- **Database:** MongoDB
- **Backend Framework:** ASP.NET Core
