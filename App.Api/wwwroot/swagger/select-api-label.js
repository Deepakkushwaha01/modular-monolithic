(() => {
    const selectorClass = "swagger-api-selector";

    const titleCase = value => value
        .split(/[-_\s]+/)
        .filter(Boolean)
        .map(part => part.charAt(0).toUpperCase() + part.slice(1))
        .join(" ");

    const getDocumentDetails = option => {
        const documentName = new URL(option.value, window.location.origin)
            .pathname
            .split("/")
            .filter(Boolean)
            .at(-2);
        const match = documentName?.match(/^(?<module>.+)-(?<version>v[^-]+)$/i);

        if (!match?.groups) {
            return null;
        }

        return {
            module: match.groups.module,
            moduleLabel: titleCase(match.groups.module),
            option,
            version: match.groups.version
        };
    };

    const createSelect = (label, ariaLabel) => {
        const field = document.createElement("label");
        field.className = `${selectorClass}__field`;
        field.textContent = label;

        const select = document.createElement("select");
        select.setAttribute("aria-label", ariaLabel);
        field.append(select);

        return { field, select };
    };

    const renderSelector = () => {
        const swaggerSelect = document.querySelector(".download-url-wrapper select");
        const swaggerWrapper = swaggerSelect?.closest(".download-url-wrapper");

        if (!swaggerSelect || !swaggerWrapper || swaggerWrapper.dataset.twoPartSelector === "true") {
            return Boolean(swaggerWrapper);
        }

        const documents = Array.from(swaggerSelect.options)
            .filter(option => option.value)
            .map(getDocumentDetails)
            .filter(Boolean);

        if (documents.length === 0) {
            return false;
        }

        const container = document.createElement("div");
        container.className = selectorClass;
        const api = createSelect("API", "Select API module");
        const version = createSelect("Version", "Select API version");
        container.append(api.field, version.field);

        const modules = [...new Map(documents.map(document => [document.module, document.moduleLabel]))];
        modules.forEach(([module, label]) => api.select.add(new Option(label, module)));

        const selectedDocument = documents.find(document => document.option.selected) ?? documents[0];
        api.select.value = selectedDocument.module;

        const setVersions = (module, selectedVersion) => {
            const availableDocuments = documents.filter(document => document.module === module);
            version.select.replaceChildren();
            availableDocuments.forEach(document => version.select.add(new Option(document.version, document.version)));
            version.select.value = availableDocuments.some(document => document.version === selectedVersion)
                ? selectedVersion
                : availableDocuments[0].version;
        };

        const loadDocument = () => {
            const document = documents.find(item =>
                item.module === api.select.value && item.version === version.select.value);

            if (!document || swaggerSelect.value === document.option.value) {
                return;
            }

            swaggerSelect.value = document.option.value;
            swaggerSelect.dispatchEvent(new Event("change", { bubbles: true }));
        };

        setVersions(selectedDocument.module, selectedDocument.version);
        api.select.addEventListener("change", () => {
            setVersions(api.select.value, version.select.value);
            loadDocument();
        });
        version.select.addEventListener("change", loadDocument);

        swaggerWrapper.dataset.twoPartSelector = "true";
        swaggerWrapper.style.display = "none";
        swaggerWrapper.parentElement?.append(container);

        return true;
    };

    const observer = new MutationObserver(() => renderSelector());
    observer.observe(document.body, { childList: true, subtree: true });
    renderSelector();
})();
