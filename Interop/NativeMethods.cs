using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Saucer.Interop
{
    /// <summary>
    /// Native P/Invoke declarations for Saucer C bindings
    /// </summary>
    internal static class NativeMethods
    {
        private const string LibraryName = "saucer";

        // ==================== Application Functions ====================

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_application_options_new(string id);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_application_options_free(IntPtr options);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_application_options_set_argc(IntPtr options, int argc);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_application_options_set_argv(IntPtr options, IntPtr[] argv);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_application_options_set_quit_on_last_window_closed(IntPtr options, bool quit);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_application_new(IntPtr options, out int error);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_application_free(IntPtr app);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool saucer_application_thread_safe(IntPtr app);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_application_screens(IntPtr app, out IntPtr screens, out UIntPtr size);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_application_post(IntPtr app, PostCallback callback, IntPtr userdata);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_application_quit(IntPtr app);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int saucer_application_run(IntPtr app, RunCallback runCallback, FinishCallback finishCallback, IntPtr userdata);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr saucer_application_on(IntPtr app, ApplicationEvent eventType, IntPtr callback, bool clearable, IntPtr userdata);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_application_once(IntPtr app, ApplicationEvent eventType, IntPtr callback, IntPtr userdata);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_application_off(IntPtr app, ApplicationEvent eventType, UIntPtr id);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_application_off_all(IntPtr app, ApplicationEvent eventType);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_application_native(IntPtr app, UIntPtr idx, IntPtr result, ref UIntPtr size);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_version();

        // ==================== Window Functions ====================

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_window_new(IntPtr app, out int error);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_free(IntPtr window);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool saucer_window_visible(IntPtr window);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool saucer_window_focused(IntPtr window);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool saucer_window_minimized(IntPtr window);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool saucer_window_maximized(IntPtr window);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool saucer_window_resizable(IntPtr window);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool saucer_window_fullscreen(IntPtr window);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool saucer_window_always_on_top(IntPtr window);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool saucer_window_click_through(IntPtr window);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_title(IntPtr window, StringBuilder title, ref UIntPtr size);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_background(IntPtr window, out byte r, out byte g, out byte b, out byte a);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int saucer_window_decorations(IntPtr window);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_size(IntPtr window, out int w, out int h);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_max_size(IntPtr window, out int w, out int h);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_min_size(IntPtr window, out int w, out int h);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_position(IntPtr window, out int x, out int y);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_window_screen(IntPtr window);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_hide(IntPtr window);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_show(IntPtr window);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_close(IntPtr window);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_focus(IntPtr window);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_start_drag(IntPtr window);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_start_resize(IntPtr window, WindowEdge edge);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_set_minimized(IntPtr window, bool minimized);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_set_maximized(IntPtr window, bool maximized);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_set_resizable(IntPtr window, bool resizable);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_set_fullscreen(IntPtr window, bool fullscreen);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_set_always_on_top(IntPtr window, bool alwaysOnTop);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_set_click_through(IntPtr window, bool clickThrough);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_set_icon(IntPtr window, IntPtr icon);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_set_title(IntPtr window, string title);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_set_background(IntPtr window, byte r, byte g, byte b, byte a);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_set_decorations(IntPtr window, WindowDecoration decoration);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_set_size(IntPtr window, int w, int h);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_set_max_size(IntPtr window, int w, int h);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_set_min_size(IntPtr window, int w, int h);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_set_position(IntPtr window, int x, int y);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr saucer_window_on(IntPtr window, WindowEvent eventType, IntPtr callback, bool clearable, IntPtr userdata);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_once(IntPtr window, WindowEvent eventType, IntPtr callback, IntPtr userdata);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_off(IntPtr window, WindowEvent eventType, UIntPtr id);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_off_all(IntPtr window, WindowEvent eventType);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_window_native(IntPtr window, UIntPtr idx, IntPtr result, ref UIntPtr size);

        // ==================== Webview Functions ====================

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_webview_options_new(IntPtr window);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_options_free(IntPtr options);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_options_set_attributes(IntPtr options, bool attributes);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_options_set_persistent_cookies(IntPtr options, bool persistent);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_options_set_hardware_acceleration(IntPtr options, bool acceleration);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_options_set_storage_path(IntPtr options, string path);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_options_set_user_agent(IntPtr options, string userAgent);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_options_append_browser_flag(IntPtr options, string flag);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_webview_new(IntPtr options, out int error);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_free(IntPtr webview);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_webview_url(IntPtr webview, out int error);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_webview_favicon(IntPtr webview);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_page_title(IntPtr webview, StringBuilder title, ref UIntPtr size);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool saucer_webview_dev_tools(IntPtr webview);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool saucer_webview_context_menu(IntPtr webview);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool saucer_webview_force_dark(IntPtr webview);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_background(IntPtr webview, out byte r, out byte g, out byte b, out byte a);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_bounds(IntPtr webview, out int x, out int y, out int w, out int h);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_set_url(IntPtr webview, IntPtr url);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_set_url_str(IntPtr webview, string url);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_set_html(IntPtr webview, string html);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_set_dev_tools(IntPtr webview, bool enabled);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_set_context_menu(IntPtr webview, bool enabled);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_set_force_dark(IntPtr webview, bool enabled);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_set_background(IntPtr webview, byte r, byte g, byte b, byte a);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_reset_bounds(IntPtr webview);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_set_bounds(IntPtr webview, int x, int y, int w, int h);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_back(IntPtr webview);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_forward(IntPtr webview);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_reload(IntPtr webview);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_serve(IntPtr webview, string scheme);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_embed(IntPtr webview, string path, IntPtr content, string mime);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_unembed_all(IntPtr webview);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_unembed(IntPtr webview, string path);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_execute(IntPtr webview, string javascript);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr saucer_webview_inject(IntPtr webview, string code, ScriptTime runAt, bool noFrames, bool clearable);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_uninject_all(IntPtr webview);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_uninject(IntPtr webview, UIntPtr id);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_handle_scheme(IntPtr webview, string scheme, SchemeHandler handler);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_remove_scheme(IntPtr webview, string scheme);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_register_scheme(string scheme);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr saucer_webview_on(IntPtr webview, WebviewEvent eventType, IntPtr callback, bool clearable, IntPtr userdata);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_once(IntPtr webview, WebviewEvent eventType, IntPtr callback, IntPtr userdata);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_off(IntPtr webview, WebviewEvent eventType, UIntPtr id);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_off_all(IntPtr webview, WebviewEvent eventType);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_webview_native(IntPtr webview, UIntPtr idx, IntPtr result, ref UIntPtr size);

        // ==================== URL Functions ====================

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_url_new_parse(string url, out int error);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_url_new_from(string url, out int error);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_url_new_opts(string scheme, string host, ref UIntPtr port, string path);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_url_free(IntPtr url);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_url_copy(IntPtr url);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_url_string(IntPtr url, StringBuilder str, ref UIntPtr size);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_url_path(IntPtr url, StringBuilder path, ref UIntPtr size);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_url_scheme(IntPtr url, StringBuilder scheme, ref UIntPtr size);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_url_host(IntPtr url, StringBuilder host, ref UIntPtr size);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool saucer_url_port(IntPtr url, ref UIntPtr port);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_url_user(IntPtr url, StringBuilder user, ref UIntPtr size);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_url_password(IntPtr url, StringBuilder password, ref UIntPtr size);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_url_native(IntPtr url, UIntPtr idx, IntPtr result, ref UIntPtr size);

        // ==================== Icon Functions ====================

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_icon_new_from_file(string path, out int error);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_icon_new_from_stash(IntPtr stash, out int error);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_icon_free(IntPtr icon);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_icon_copy(IntPtr icon);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool saucer_icon_empty(IntPtr icon);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_icon_data(IntPtr icon);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_icon_save(IntPtr icon, string path);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_icon_native(IntPtr icon, UIntPtr idx, IntPtr result, ref UIntPtr size);

        // ==================== Stash Functions ====================

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_stash_new_from(IntPtr data, UIntPtr size);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_stash_new_view(IntPtr data, UIntPtr size);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_stash_new_lazy(StashLazyCallback callback, IntPtr userdata);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_stash_new_from_str(string str);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_stash_new_view_str(string str);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_stash_new_empty();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_stash_free(IntPtr stash);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_stash_copy(IntPtr stash);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_stash_data(IntPtr stash);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr saucer_stash_size(IntPtr stash);

        // ==================== Permission Functions ====================

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_permission_request_free(IntPtr request);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_permission_request_copy(IntPtr request);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_permission_request_url(IntPtr request);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern PermissionType saucer_permission_request_type(IntPtr request);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_permission_request_accept(IntPtr request, bool accept);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_permission_request_native(IntPtr request, UIntPtr idx, IntPtr result, ref UIntPtr size);

        // ==================== Scheme Functions ====================

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_scheme_response_free(IntPtr response);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_scheme_response_new(IntPtr stash, string mime);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_scheme_response_append_header(IntPtr response, string key, string value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_scheme_response_set_status(IntPtr response, int status);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_scheme_request_free(IntPtr request);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_scheme_request_copy(IntPtr request);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_scheme_request_url(IntPtr request);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_scheme_request_method(IntPtr request, StringBuilder method, ref UIntPtr size);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_scheme_request_content(IntPtr request);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_scheme_request_headers(IntPtr request, StringBuilder headers, ref UIntPtr size);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_scheme_executor_free(IntPtr executor);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_scheme_executor_copy(IntPtr executor);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_scheme_executor_reject(IntPtr executor, SchemeError error);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_scheme_executor_accept(IntPtr executor, IntPtr response);

        // ==================== Screen Functions ====================

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_screen_free(IntPtr screen);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr saucer_screen_name(IntPtr screen);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_screen_size(IntPtr screen, out int w, out int h);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void saucer_screen_position(IntPtr screen, out int x, out int y);

        // ==================== Enumerations ====================

        public enum Policy : byte
        {
            Allow,
            Block
        }

        public enum ApplicationEvent
        {
            Quit
        }

        public enum WindowEvent
        {
            Decorated = 0,
            Maximize = 1,
            Minimize = 2,
            Closed = 3,
            Resize = 4,
            Focus = 5,
            Close = 6
        }

        public enum WindowEdge : byte
        {
            Top = 1 << 0,
            Bottom = 1 << 1,
            Left = 1 << 2,
            Right = 1 << 3,
            BottomLeft = Bottom | Left,
            BottomRight = Bottom | Right,
            TopLeft = Top | Left,
            TopRight = Top | Right
        }

        public enum WindowDecoration : byte
        {
            None = 0,
            Partial = 1,
            Full = 2
        }

        public enum WebviewEvent
        {
            Permission = 0,
            Fullscreen = 1,
            DomReady = 2,
            Navigated = 3,
            Navigate = 4,
            Message = 5,
            Request = 6,
            Favicon = 7,
            Title = 8,
            Load = 9
        }

        public enum ScriptTime
        {
            Creation,
            Ready
        }

        public enum State
        {
            Started,
            Finished
        }

        public enum Status
        {
            Handled,
            Unhandled
        }

        public enum PermissionType : byte
        {
            Unknown = 0,
            AudioMedia = 1 << 0,
            VideoMedia = 1 << 1,
            DesktopMedia = 1 << 2,
            MouseLock = 1 << 3,
            DeviceInfo = 1 << 4,
            Location = 1 << 5,
            Clipboard = 1 << 6,
            Notification = 1 << 7
        }

        public enum SchemeError : short
        {
            NotFound = 404,
            Invalid = 400,
            Denied = 401,
            Failed = -1
        }

        // ==================== Delegates ====================

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void PostCallback(IntPtr userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void RunCallback(IntPtr app, IntPtr userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void FinishCallback(IntPtr app, IntPtr userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Policy ApplicationQuitCallback(IntPtr app, IntPtr userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void WindowDecoratedCallback(IntPtr window, WindowDecoration decoration, IntPtr userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void WindowMaximizeCallback(IntPtr window, bool maximized, IntPtr userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void WindowMinimizeCallback(IntPtr window, bool minimized, IntPtr userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void WindowClosedCallback(IntPtr window, IntPtr userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void WindowResizeCallback(IntPtr window, int w, int h, IntPtr userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void WindowFocusCallback(IntPtr window, bool focused, IntPtr userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Policy WindowCloseCallback(IntPtr window, IntPtr userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Status WebviewPermissionCallback(IntPtr webview, IntPtr request, IntPtr userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Policy WebviewFullscreenCallback(IntPtr webview, bool fullscreen, IntPtr userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void WebviewDomReadyCallback(IntPtr webview, IntPtr userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void WebviewNavigatedCallback(IntPtr webview, IntPtr url, IntPtr userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Policy WebviewNavigateCallback(IntPtr webview, IntPtr navigation, IntPtr userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Status WebviewMessageCallback(IntPtr webview, string message, UIntPtr size, IntPtr userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void WebviewRequestCallback(IntPtr webview, IntPtr url, IntPtr userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void WebviewFaviconCallback(IntPtr webview, IntPtr icon, IntPtr userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void WebviewTitleCallback(IntPtr webview, string title, UIntPtr size, IntPtr userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void WebviewLoadCallback(IntPtr webview, State state, IntPtr userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SchemeHandler(IntPtr request, IntPtr executor);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr StashLazyCallback(IntPtr userdata);
    }
}
