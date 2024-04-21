import csv
import pandas as pd
import pyodbc
from config import server, database, username, password, driver
connection_string = f'DRIVER={driver};SERVER={server};DATABASE={database};UID={username};PWD={password}'

csv_file_path = 'TableDataInsert\BigBasketProducts.csv'
df = pd.read_csv(csv_file_path)

conn = pyodbc.connect(connection_string)
cursor = conn.cursor()

# Read the CSV file and insert the data into the database
cursor.execute('''
    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ChromaElectronics')
    CREATE TABLE ChromaElectronics (
        [index] INT PRIMARY KEY IDENTITY(1,1),
        link NVARCHAR(MAX),
        price NVARCHAR(MAX),
        name NVARCHAR(MAX),
        category NVARCHAR(MAX),
        features NVARCHAR(MAX),
        overview NVARCHAR(MAX),
        images NVARCHAR(MAX),
        specification NVARCHAR(MAX)
    )
''')

uploaded = True
# Read the CSV file and insert the data into the database
with open(csv_file_path, newline='', encoding='utf-8') as csvfile:
    csvreader = csv.reader(csvfile)
    next(csvreader)  # Skip the header row
    for idx, row in enumerate(csvreader, start=1):
        # row = [None if value == '' else value for value in row]
        for i in range(len(row)):
            if row[i] == "": row[i] = None
        try:
            cursor.execute("INSERT INTO ChromaElectronics (link, price, name, category, features, overview, images, specification) VALUES (?, ?, ?, ?, ?, ?, ?, ?)", row[2:])
        except pyodbc.Error as e:
            print(f"Error inserting row {idx}: {e}")
            print(f"Row data: {row}")
            uploaded = False
            break


if uploaded: 
    conn.commit()
    print(f'{csv_file_path} CSV file uploaded to SQL Server database successfully.')
