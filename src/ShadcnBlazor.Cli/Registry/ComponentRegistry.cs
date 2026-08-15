using System;
using System.Collections.Generic;

namespace ShadcnBlazor.Cli.Registry;

public record RegistryComponent(
    string Name,
    string Description,
    string[] Dependencies,
    string[] Files
);

public static class ComponentRegistry
{
    public static readonly Dictionary<string, RegistryComponent> Components = new(StringComparer.OrdinalIgnoreCase)
    {
        ["button"] = new("button", "Interactive button with variants, sizes, and states", Array.Empty<string>(), new[] { "Button/Button.razor" }),
        ["badge"] = new("badge", "Compact status or indicator badge", Array.Empty<string>(), new[] { "Badge/Badge.razor" }),
        ["card"] = new("card", "Structured container with header, content, footer", Array.Empty<string>(), new[] { "Card/Card.razor", "Card/CardHeader.razor", "Card/CardTitle.razor", "Card/CardDescription.razor", "Card/CardContent.razor", "Card/CardFooter.razor" }),
        ["input"] = new("input", "Form input field with icon slots", Array.Empty<string>(), new[] { "Input/Input.razor" }),
        ["textarea"] = new("textarea", "Multiline text input field", Array.Empty<string>(), new[] { "Textarea/Textarea.razor" }),
        ["label"] = new("label", "Accessible typographic label", Array.Empty<string>(), new[] { "Label/Label.razor" }),
        ["checkbox"] = new("checkbox", "Accessible checkbox with label/description", Array.Empty<string>(), new[] { "Checkbox/Checkbox.razor" }),
        ["switch"] = new("switch", "Smooth toggle switch component", Array.Empty<string>(), new[] { "Switch/Switch.razor" }),
        ["radio-group"] = new("radio-group", "Accessible radio button group", Array.Empty<string>(), new[] { "RadioGroup/RadioGroup.razor", "RadioGroup/RadioGroupItem.razor" }),
        ["alert"] = new("alert", "Callout alert banner for feedback", Array.Empty<string>(), new[] { "Alert/Alert.razor", "Alert/AlertTitle.razor", "Alert/AlertDescription.razor" }),
        ["dialog"] = new("dialog", "Accessible modal dialog overlay", new[] { "button" }, new[] { "Dialog/Dialog.razor", "Dialog/DialogTrigger.razor", "Dialog/DialogContent.razor", "Dialog/DialogHeader.razor", "Dialog/DialogTitle.razor", "Dialog/DialogDescription.razor", "Dialog/DialogFooter.razor", "Dialog/DialogClose.razor" }),
        ["alert-dialog"] = new("alert-dialog", "Confirmation dialog interrupting user workflow", new[] { "button" }, new[] { "AlertDialog/AlertDialog.razor", "AlertDialog/AlertDialogTrigger.razor", "AlertDialog/AlertDialogContent.razor", "AlertDialog/AlertDialogHeader.razor", "AlertDialog/AlertDialogTitle.razor", "AlertDialog/AlertDialogDescription.razor", "AlertDialog/AlertDialogFooter.razor", "AlertDialog/AlertDialogAction.razor", "AlertDialog/AlertDialogCancel.razor" }),
        ["sheet"] = new("sheet", "Slide-in panel from any screen edge", new[] { "button" }, new[] { "Sheet/Sheet.razor", "Sheet/SheetTrigger.razor", "Sheet/SheetContent.razor", "Sheet/SheetHeader.razor", "Sheet/SheetTitle.razor", "Sheet/SheetDescription.razor", "Sheet/SheetFooter.razor" }),
        ["dropdown-menu"] = new("dropdown-menu", "Floating action menu and submenus", Array.Empty<string>(), new[] { "DropdownMenu/DropdownMenu.razor", "DropdownMenu/DropdownMenuTrigger.razor", "DropdownMenu/DropdownMenuContent.razor", "DropdownMenu/DropdownMenuItem.razor", "DropdownMenu/DropdownMenuCheckboxItem.razor", "DropdownMenu/DropdownMenuSeparator.razor", "DropdownMenu/DropdownMenuLabel.razor", "DropdownMenu/DropdownMenuShortcut.razor" }),
        ["popover"] = new("popover", "Rich content anchored to a trigger element", Array.Empty<string>(), new[] { "Popover/Popover.razor", "Popover/PopoverTrigger.razor", "Popover/PopoverContent.razor" }),
        ["tooltip"] = new("tooltip", "Hover / focus popup description", Array.Empty<string>(), new[] { "Tooltip/Tooltip.razor", "Tooltip/TooltipTrigger.razor", "Tooltip/TooltipContent.razor" }),
        ["tabs"] = new("tabs", "Tabbed interface panels", Array.Empty<string>(), new[] { "Tabs/Tabs.razor", "Tabs/TabsList.razor", "Tabs/TabsTrigger.razor", "Tabs/TabsContent.razor" }),
        ["accordion"] = new("accordion", "Vertically stacked interactive disclosure headings", Array.Empty<string>(), new[] { "Accordion/Accordion.razor", "Accordion/AccordionItem.razor", "Accordion/AccordionTrigger.razor", "Accordion/AccordionContent.razor" }),
        ["avatar"] = new("avatar", "Image avatar with fallback initials", Array.Empty<string>(), new[] { "Avatar/Avatar.razor", "Avatar/AvatarImage.razor", "Avatar/AvatarFallback.razor" }),
        ["skeleton"] = new("skeleton", "Pulsing content placeholder", Array.Empty<string>(), new[] { "Skeleton/Skeleton.razor" }),
        ["progress"] = new("progress", "Determinate and indeterminate progress bar", Array.Empty<string>(), new[] { "Progress/Progress.razor" }),
        ["slider"] = new("slider", "Range slider input", Array.Empty<string>(), new[] { "Slider/Slider.razor" }),
        ["separator"] = new("separator", "Horizontal or vertical visual divider", Array.Empty<string>(), new[] { "Separator/Separator.razor" }),
        ["table"] = new("table", "Responsive semantic data table", Array.Empty<string>(), new[] { "Table/Table.razor", "Table/TableHeader.razor", "Table/TableBody.razor", "Table/TableFooter.razor", "Table/TableRow.razor", "Table/TableHead.razor", "Table/TableCell.razor", "Table/TableCaption.razor" }),
        ["breadcrumb"] = new("breadcrumb", "Hierarchical navigation path trail", Array.Empty<string>(), new[] { "Breadcrumb/Breadcrumb.razor", "Breadcrumb/BreadcrumbList.razor", "Breadcrumb/BreadcrumbItem.razor", "Breadcrumb/BreadcrumbLink.razor", "Breadcrumb/BreadcrumbPage.razor", "Breadcrumb/BreadcrumbSeparator.razor" }),
        ["select"] = new("select", "Custom dropdown select with item highlights", Array.Empty<string>(), new[] { "Select/Select.razor", "Select/SelectTrigger.razor", "Select/SelectValue.razor", "Select/SelectContent.razor", "Select/SelectItem.razor", "Select/SelectLabel.razor", "Select/SelectSeparator.razor" }),
        ["toast"] = new("toast", "Sonner-style stacked toast notifications", Array.Empty<string>(), new[] { "Toast/Toaster.razor", "Toast/ToastItem.razor" }),
        ["form"] = new("form", "Accessible form layout and submission primitives", Array.Empty<string>(), new[] { "Form/Form.razor", "Form/FormField.razor" }),
        ["calendar"] = new("calendar", "Accessible date calendar with range constraints", Array.Empty<string>(), new[] { "Calendar/Calendar.razor" }),
        ["date-picker"] = new("date-picker", "Popover date selection input", new[] { "calendar", "popover", "button" }, new[] { "DatePicker/DatePicker.razor" }),
        ["command"] = new("command", "Searchable command palette primitives", Array.Empty<string>(), new[] { "Command/Command.razor", "Command/CommandInput.razor", "Command/CommandList.razor", "Command/CommandEmpty.razor", "Command/CommandGroup.razor", "Command/CommandItem.razor" }),
        ["combobox"] = new("combobox", "Searchable single-value selection control", Array.Empty<string>(), new[] { "Combobox/Combobox.razor", "Combobox/ComboboxItem.razor" }),
        ["pagination"] = new("pagination", "Accessible page navigation controls", Array.Empty<string>(), new[] { "Pagination/Pagination.razor", "Pagination/PaginationItem.razor" }),
        ["toggle"] = new("toggle", "Pressed state button and toggle groups", Array.Empty<string>(), new[] { "Toggle/Toggle.razor", "Toggle/ToggleGroup.razor", "Toggle/ToggleGroupItem.razor", "Toggle/ToggleVariants.cs" }),
        ["collapsible"] = new("collapsible", "Expandable and collapsible content region", Array.Empty<string>(), new[] { "Collapsible/Collapsible.razor", "Collapsible/CollapsibleTrigger.razor", "Collapsible/CollapsibleContent.razor" }),
        ["context-menu"] = new("context-menu", "Right-click contextual action menu", Array.Empty<string>(), new[] { "ContextMenu/ContextMenu.razor", "ContextMenu/ContextMenuTrigger.razor", "ContextMenu/ContextMenuContent.razor", "ContextMenu/ContextMenuItem.razor", "ContextMenu/ContextMenuSeparator.razor" }),
        ["navigation-menu"] = new("navigation-menu", "Accessible multi-level site navigation", Array.Empty<string>(), new[] { "NavigationMenu/NavigationMenu.razor", "NavigationMenu/NavigationMenuList.razor", "NavigationMenu/NavigationMenuItem.razor", "NavigationMenu/NavigationMenuTrigger.razor", "NavigationMenu/NavigationMenuContent.razor" }),
        ["menubar"] = new("menubar", "Desktop application-style menu bar", Array.Empty<string>(), new[] { "Menubar/Menubar.razor", "Menubar/MenubarMenu.razor", "Menubar/MenubarTrigger.razor", "Menubar/MenubarContent.razor", "Menubar/MenubarItem.razor", "Menubar/MenubarSeparator.razor" }),
        ["carousel"] = new("carousel", "Keyboard-friendly sliding content carousel", Array.Empty<string>(), new[] { "Carousel/Carousel.razor", "Carousel/CarouselContent.razor", "Carousel/CarouselItem.razor", "Carousel/CarouselPrevious.razor", "Carousel/CarouselNext.razor" })
    };
}
