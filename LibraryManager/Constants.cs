namespace LibraryManager;

/// <summary>
/// A static class containing constants used throughout the application.
/// </summary>
/// <author>YR 2025-02-16</author>
public static class Constants
{
    #region General
    public const string LIBRARY_MANAGER = "LibraryManager";
    public const string LIBRARY_MANAGER_PIPE_SERVER = "LibraryManagerPipeServer";
    #endregion

    #region ToolsViewModel
    public const string TOOLS = "Tools";
    #endregion

    #region StatusBarViewModel
    public const string TOTAL_BOOKS_IN_LIBRARY = "Total books in the library";
    #endregion

    #region LibraryViewModel
    public const string LIBRARY = "Library";
    public const string LIBRARY_WAS_CREATED_SUCCESSFULLY = "Library was created successfully";
    public const string LOADING_LIBRARY_FROM_XML = "Loading library from the XML file...";
    public const string LIBRARY_WAS_LOADED_SUCCESSFULLY = "Library was loaded successfully from path";
    public const string LIBRARY_LOADED_WITH_ID = "Library loaded with ID";
    public const string FAILED_TO_LOAD_LIBRARY_FROM_PATH = "Failed to load library from path";
    public const string LIBRARY_LOADING_FINISHED = "Library loading finished";
    public const string SAVE_LIBRARY_DIALOG = "Save Library Dialog";
    public const string FOLDER_WAS_NOT_SELECTED = "Folder was not selected";
    public const string LIBRARY_IS_BEING_SAVED = "The library is being saved...";
    public const string LIBRARY_WAS_SAVED_SUCCESSFULLY = "Library was saved successfully to path";
    public const string FAILED_TO_SAVE_LIBRARY_TO_PATH = "Failed to save library to path";
    public const string LIBRARY_WAS_CLOSED = "Library was closed";
    #endregion

    #region book details: FindBookViewModel, CreatorBookDetailsViewModel, EditorBookDetailsViewModel
    public const string ID = "ID";
    public const string FIND_BOOKS = "Find Books";
    public const string BOOK_WAS_DELETED_SUCCESSFULLY = "Book was deleted successfully";
    public const string NO_BOOKS_FOUND = "No books found";
    public const string ADD_BOOK = "Add Book";
    public const string LAST_ADDED_BOOK = "Last added book";
    public const string ADDING_BOOK_WAS_CANCELLED = "Adding book was cancelled";
    public const string EDIT_BOOK = "Edit Book";
    public const string ORIGINAL_STATE_BOOK = "Original state of the book";
    public const string LOADED_FOR_EDITING = "is loaded for editing";
    public const string LAST_EDITED_BOOK = "Last edited book";
    public const string EDITING_BOOK_WAS_CANCELLED = "Editing book was cancelled";

    public const string CONTENT_WAS_LOADED = "Content was loaded";
    public const string LOAD_CONTENT = "Load content";
    public const string LOADING_STARTED = "Loading started";
    public const string NEW_CONTENT_WAS_LOADED_INTO_BOOK = "New content was loaded into the book";
    public const string CONTENT_WAS_NOT_LOADED_SUCCESSFULLY = "The content was not loaded successfully";
    public const string ATTEMPT_TO = "Attempt to";
    public const string SAVE_CHANGES = "Save changes";
    public const string SAVE_CONTENT = "save content";
    public const string SAVING_STARTED = "Saving started";
    public const string CONTENT_SAVED_SUCCESSFULLY = "Content saved successfully";
    public const string FAILED_TO_SAVE_CONTENT = "Failed to save content";
    public const string NO_CONTENT_TO_SAVE = "No content to save";
    #endregion

    #region DebugViewModel
    public const string DEBUG = "Debug";
    #endregion

    #region BooksViewModel
    public const string BOOKS = "Books";
    #endregion

    #region AboutViewModel
    public const string ABOUT = "About";
    #endregion
}
