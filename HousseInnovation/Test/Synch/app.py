# app.py

import streamlit as st

# Navigation entre les pages
Dashboard_page = st.Page("dashboard.py", title="Tableau de bord ", icon=":material/dashboard:")
Usage_page = st.Page("Usage.py", title="Audience", icon="📈")
Engagement_page = st.Page("Engagement.py", title="Données analytiques", icon="📲")
Maps_page = st.Page("Maps.py", title="Données géographique ", icon="🌐")

pg = st.navigation([Dashboard_page, Usage_page, Engagement_page, Maps_page])
st.set_page_config(page_title="Data manager", page_icon=":bar_chart:")
pg.run()
