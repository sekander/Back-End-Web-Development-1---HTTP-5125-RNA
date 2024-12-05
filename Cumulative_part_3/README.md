# Teacher Edit Application

This application provides a web-based interface for editing teacher information. It includes both frontend and backend components, allowing users to select and edit teacher details such as employee number, first name, last name, hire date, and salary.

## Features

- **Frontend**: A form that allows users to select a teacher from a dropdown and edit their details.
- **Backend**: API endpoints to handle editing teacher details, including validation and database updates.
- **Data Validation**: Ensures that fields like first name, last name, hire date, and salary are valid before submitting.
- **Dynamic Data Population**: The dropdown list of teachers is populated dynamically, and the form is filled with the selected teacher's information.
- **API Communication**: Frontend communicates with the backend using HTTP requests to update teacher information.

---

## Backend Implementation

### Teacher Edit API (`PUT /api/teacherapi/teacheredit?teacherID={teacherID}`)

This API endpoint allows you to edit the details of a specific teacher.

#### Request Parameters:
- **teacherID**: The ID of the teacher whose details you want to update.
- **editTeacher**: A JSON object containing the updated values for the teacher.

#### Functionality:
- Validates the request data:
  - First and last names must not be empty.
  - Hire date cannot be in the future.
  - Salary cannot be negative.
- If validation passes, an SQL query updates the teacher's details in the database.
- Responds with a success or failure message.

#### Example Request:
```bash
curl -X PUT "http://localhost:5074/api/teacherapi/teacheredit?teacherID=21" -H "Content-Type: application/json" -k -d '{
  "Id": 21,
  "EmployeeNumber": "T0123456",
  "FirstName": "Bahif",
  "LastName": "sdf",
  "HireDate": "1111-11-11T00:00:00",
  "Salary": 50000.00
}'
```


# Edit Teacher View (GET /api/teacherpage/edit)

This endpoint returns a view for editing teacher information. It provides the necessary data (teacher IDs and full teacher data) in JSON format to the frontend.

## Functionality:
- The server retrieves teacher data and serializes it into JSON.
- The view is populated with a form that allows users to select and edit a teacher's details.
- This view can be loaded dynamically via AJAX on the frontend.

## Example Request:


## Example Response:
- A dropdown of teacher IDs and a form pre-filled with selected teacher data.

## Frontend Implementation

The frontend allows users to interact with the teacher data through a form. The form includes fields for:

- Employee Number
- First Name
- Last Name
- Hire Date
- Salary

The teacher's details are dynamically populated into the form when a teacher is selected from the dropdown. Once the user makes changes and submits the form, it sends the updated data to the backend API.

## JavaScript Functionality:
- **Populate Dropdown**: The dropdown is populated dynamically with teacher IDs fetched from the backend.
- **Form Validation**:
    - Ensures that the first and last names are not empty.
    - Ensures the hire date is not in the future.
    - Ensures the salary is non-negative.
- **Submit Data**: When the form is submitted, it sends a PUT request to the backend API to update the teacher's data.

### Example JavaScript Code:

```javascript
fetch(url, {
  method: 'PUT',
  headers: {
    'Content-Type': 'application/json',
  },
  body: JSON.stringify(selectedTeacher),
})
.then(response => response.json())
.then(responseData => {
  if (responseData.success) {
    responseMessage.textContent = responseData.message;
    responseMessage.style.color = "green";
  } else {
    responseMessage.textContent = responseData.message;
    responseMessage.style.color = "red";
        }
})
.catch(error => console.error('Error:', error));
```

### Key Data Flow 
#### Frontend:

  -  The frontend sends a GET request to /api/teacherpage/edit to retrieve teacher data.
  -  It receives JSON data (teacher IDs and full teacher list) and populates the dropdown and form fields.
  -  On form submission, the frontend sends a PUT request with the updated teacher information.
   
#### Backend:
   
  - Handles the GET and PUT requests.
  - The GET request sends serialized teacher data to the frontend.
  - The PUT request validates the data, updates the teacher information in the database, and returns a response indicating success or failure.
   
#### Example Flow:
   
 -   A user accesses the teacher edit page.
 -   The page fetches the teacher IDs and teacher data from the backend.
 -   The dropdown is populated with teacher IDs. When a teacher is selected, their details are pre-filled into the form.
 -   The user edits the details and submits the form.
 -   The frontend validates the inputs before sending a PUT request with the updated teacher information.
 -   The backend processes the request and updates the teacher data in the database.
 -   The frontend displays a success or failure message based on the backend response.

#### Conclusion

- This application provides a seamless interface for editing teacher information. The backend API handles the data retrieval and updating logic, while the frontend allows users to interact with the data through a dynamic form. Together, they provide a functional system for managing teacher data in a web application.

