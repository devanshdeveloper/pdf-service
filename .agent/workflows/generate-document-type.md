# generate-document-type.md
Add support for a new document type to the Document Rendering Platform,
following the standing rules in document-engine-standards.

Inputs to ask the user for if not provided:
- Document type name (e.g. "Purchase Order")
- List of templates needed for it (e.g. Standard, Export, Manufacturing)
- Sample PDF or JSON payload, if available

Steps:
1. Register the document type in the Document Registry (do not branch
   in code — configuration/registry entry only).
2. Identify which existing reusable QuestPDF components cover its
   layout; only build new components for genuinely new sections.
3. Implement the template(s) via the Template Resolver + Renderer
   Factory pattern — never a new if/switch branch.
4. Add validation rules for the expected JSON payload shape.
5. Add unit tests for the new renderer/components and an integration
   test that generates a real PDF end-to-end.
6. Generate a sample PDF and, if a reference sample was provided,
   compare layout/spacing/typography against it.
7. Update API docs / Swagger / Postman collection with the new
   endpoint or payload variant.
8. Report back: what was added, which components were reused vs.
   newly created, and links to the generated sample PDF(s).
